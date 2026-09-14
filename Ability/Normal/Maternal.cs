using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using Sirenix.Utilities;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace RevenantMod;

public class Maternal : AbilityBehaviour
{
	public static Ability ability;
	public override Ability Ability => ability;

	private static readonly CardModificationInfo SubmergeMod = new(Ability.Submerge);

	private bool IsValidCard(PlayableCard card)
	{
		// putting these checks into a separate bool so the code looks nicer, could all be put into one line if you want
		bool isValid = !card.HasAbility(Ability.Submerge) && !card.TemporaryMods.Contains(SubmergeMod) && !card.HasAbility(ability);

		// not null, same side of the board, does not have Submerge, Maternal, or SubmergeMod
		return card != null && card.OpponentCard == Card.OpponentCard && isValid;
	}

	private IEnumerator AddSubmerge(PlayableCard playableCard)
	{
		playableCard.Anim.PlayTransformAnimation();
		yield return new WaitForSeconds(0.25f);
		playableCard.AddTemporaryMod(SubmergeMod);
	}

	private IEnumerator RevertSubmerge(PlayableCard playableCard)
	{
		if (playableCard.FaceDown)
		{
			playableCard.SetFaceDown(false);
			playableCard.UpdateFaceUpOnBoardEffects();
		}
		playableCard.Anim.PlayTransformAnimation();
		yield return new WaitForSeconds(0.25f);
		playableCard.RemoveTemporaryMod(SubmergeMod);
	}

	public override bool RespondsToResolveOnBoard() => true;

	public override IEnumerator OnResolveOnBoard()
	{
		// this just tells the game that this ability is triggering
		// it also does a null check on the base card, so that's neat
		yield return base.PreSuccessfulTriggerSequence();

		// got rid of the IEnumerator AddSubmergeToCards since it was only being used here
		foreach (CardSlot slot in Singleton<BoardManager>.Instance.GetAdjacentSlots(base.Card.Slot).Where((CardSlot s) => s.Card != null))
		{
			if (IsValidCard(slot.Card))
			{
				yield return AddSubmerge(slot.Card);
				yield return base.LearnAbility(0.5f);
			}
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
				yield return AddSubmerge(otherCard);
		}
	}

	public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer) => true;

	public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
	{
		yield return base.PreSuccessfulTriggerSequence();

		// for each adjacent card that isn't null
		foreach (CardSlot slot in Singleton<BoardManager>.Instance.GetAdjacentSlots(Card.Slot).Where((CardSlot s) => s.Card != null))
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
			if (revert)
				yield return RevertSubmerge(slot.Card);
		}
	}

	// only respond if otherCard has the SubmergeMod
	public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard) => otherCard != null && otherCard.OpponentCard == Card.OpponentCard && otherCard.TemporaryMods.Contains(SubmergeMod);

	public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
	{
		foreach (CardSlot slot in Singleton<BoardManager>.Instance.GetAdjacentSlots(otherCard.Slot).Where((CardSlot s) => s.Card != null))
			if (slot.Card.HasAbility(ability))
				yield break;
		yield return RevertSubmerge(otherCard);
	}
}

public partial class RevenantMod
{
	public void Add_Ability_Maternal()
	{
		const string rulebookName = nameof(Maternal);
		const string rulebookDescription = "Creatures adjacent to [creature] are inscrybed by Waterborne.";

		AbilityInfo ability = AbilityManager.New(
			PluginGUID,
			rulebookName.SplitPascalCase(),
			rulebookDescription,
			typeof(Maternal),
			AbilitiesUtil.LoadAbilityIcon(Ability.Sinkhole.ToString())
			)
			.SetDefaultPart1Ability()
			.AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel2"))
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))
            ;
		ability.powerLevel = 3;
		Maternal.ability = ability.ability;
	}
}
