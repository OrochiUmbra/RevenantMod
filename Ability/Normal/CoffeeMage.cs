/*using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace RevenantMod;
public class CoffeeMage : AbilityBehaviour
{
	public static Ability ability;
	public override Ability Ability => ability;

	private PlayableCard OpposingCard => Card.Slot.opposingSlot.Card;

	private CardModificationInfo NegateMod
	{
		get
		{
			CardModificationInfo cardModificationInfo = new() { negateAbilities = new List<Ability>() };
			if (OpposingCard.TemporaryMods != null)
				foreach (CardModificationInfo cardModificationInfo2 in OpposingCard.TemporaryMods)
					cardModificationInfo.negateAbilities.AddRange(cardModificationInfo2.abilities);
			if (OpposingCard.Info.Abilities != null)
				cardModificationInfo.negateAbilities.AddRange(OpposingCard.Info.Abilities);
			return cardModificationInfo;
		}
	}

	private bool IsValidCard(PlayableCard card)
	{
		// putting these checks into a separate bool so the code looks nicer, could all be put into one line if you want
		bool isValid = !card.TemporaryMods.Contains(NegateMod) && !card.HasAbility(ability);

		// not null, same side of the board, does not have Submerge, Maternal, or SubmergeMod
		return card != null && card.OpponentCard != Card.OpponentCard && isValid;
	}

	private IEnumerator AddNegate(PlayableCard playableCard)
	{
		playableCard.Anim.PlayTransformAnimation();
		yield return new WaitForSeconds(0.25f);
		playableCard.AddTemporaryMod(NegateMod);
	}

	private IEnumerator RevertNegate(PlayableCard playableCard)
	{
		if (playableCard.FaceDown)
		{
			playableCard.SetFaceDown(false);
			playableCard.UpdateFaceUpOnBoardEffects();
		}
		playableCard.Anim.PlayTransformAnimation();
		yield return new WaitForSeconds(0.25f);
		playableCard.RemoveTemporaryMod(NegateMod);
	}

	public override bool RespondsToResolveOnBoard() => OpposingCard != null;

	public override IEnumerator OnResolveOnBoard()
	{
		// this just tells the game that this ability is triggering
		// it also does a null check on the base card, so that's neat
		yield return base.PreSuccessfulTriggerSequence();

		// got rid of the IEnumerator AddSubmergeToCards since it was only being used here
		if (IsValidCard(OpposingCard) && !OpposingCard.HasAbility(ability))
		{
			yield return AddNegate(OpposingCard);
			yield return base.LearnAbility(0.5f);
		}
	}

	public override bool RespondsToOtherCardResolve(PlayableCard otherCard) => IsValidCard(otherCard);

	public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
	{
		yield return base.PreSuccessfulTriggerSequence();

		// check adjacent slots for otherCard
		foreach (CardSlot slot in Singleton<BoardManager>.Instance.GetAdjacentSlots(Card.Slot).Where((CardSlot s) => s.Card != null))
		{
			if (slot.Card == otherCard)
				yield return AddNegate(otherCard);
		}
	}

	public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer) => OpposingCard != null;

	public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
	{
		yield return base.PreSuccessfulTriggerSequence();

		// for each adjacent card that isn't null
		foreach (CardSlot slot in Singleton<BoardManager>.Instance.AllSlotsCopy.Where(slot => slot.Card != null && !slot.Card.Dead && slot.Card != Card.OpponentCard && slot.Card != IsValidCard(OpposingCard)))
        {
			bool revert = true;
			// for each adjacent card that isn't this card (1 slot)
			foreach (CardSlot _slot in Singleton<BoardManager>.Instance.GetAdjacentSlots(slot).Where((CardSlot s) => s.Card != null))
			{
				// if not null and has Maternal, revert = false
				if (_slot.Card != Card && _slot.Card.HasAbility(ability))
				{
					revert = false;
				}
			}
			// if no other adjacent Maternal card, revert Submerge
			if (!OpposingCard.Dead && OpposingCard.TemporaryMods.Contains(NegateMod) && !OpposingCard.HasAbility(ability))
				yield return RevertNegate(OpposingCard);
		}
	}

	// only respond if otherCard has the SubmergeMod
	public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard) => otherCard.OpposingCard != null && otherCard.OpponentCard != Card.OpponentCard && otherCard.TemporaryMods.Contains(NegateMod);

	public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
	{
		foreach (CardSlot slot in Singleton<BoardManager>.Instance.AllSlotsCopy.Where(slot => slot.Card != null && !slot.Card.Dead && slot.Card == IsValidCard(OpposingCard)))
            if (slot.Card.HasAbility(ability))
				yield break;
		if (IsValidCard(otherCard) && !otherCard.HasAbility(ability))
			yield return AddNegate(otherCard);
	}

    [HarmonyPatch]
    internal class SentryRemoveFix
    {
        [HarmonyPrefix, HarmonyPatch(typeof(AbilityBehaviour), nameof(AbilityBehaviour.Card), MethodType.Getter)]
        private static bool DummyCardWorkAround(AbilityBehaviour __instance, ref PlayableCard __result)
        {
            // null check doesn't work so we use try-catch
            try
            {
                // vanilla code; this is what's causing the error
                // essentially negateAbilities removes ability triggers by destroying the associated object
                // so the error results from trying to get components of a non-existent object (i think)
                __result = __instance.GetComponent<PlayableCard>();
            }
            catch
            {
                // if we get an error, provide a dummy PlayableCard with an empty CardInfo
                // this handles Sentry's NumShots check, which looks at the CardInfo
                __result = new() { Info = ScriptableObject.CreateInstance<CardInfo>() };
            }
            return false;
        }
    }
}

public partial class RevenantMod
{
	public void Add_Ability_CoffeeMage()
	{
        {
            const string rulebookName = nameof(CoffeeMage);
            const string rulebookDescription = "Creature opposing to [creature] loses all of its sigils.";

            AbilityInfo ability = AbilityManager.New(
                PluginGUID,
                rulebookName.SplitPascalCase(),
                rulebookDescription,
                typeof(CoffeeMage),
                Loader.LoadIcon(nameof(Torpid))
                )
                .SetDefaultPart1Ability()
                .AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel3"))
                .SetPixelAbilityIcon(Loader.LoadIcon(nameof(Torpid), true))
                ;
            ability.powerLevel = 3;
            CoffeeMage.ability = ability.ability;
        }*/
        /*const string rulebookName = nameof(CoffeeMage);
        const string rulebookDescription = "Creature opposing to [creature] loses all of its sigils.";

		AbilityBuilder<CoffeeMage>.Builder
		.SetIcon(Asset.LoadIcon(nameof(PartingStroke)))
		.SetRulebookDescription(rulebookDescription)
		.Build();*/
	//}
//}
