using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using InscryptionAPI.Helpers.Extensions;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;

namespace RevenantMod;

public class SplittingCorpse : AbilityBehaviour
{
    public static Ability ability;
    public override Ability Ability => ability;

    public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer) => !wasSacrifice;
    private IEnumerator Effect()
    {
        CardSlot leftSlot = base.Card.Slot.GetAdjacent(true);
        CardSlot rightSlot = base.Card.Slot.GetAdjacent(false);
        CardSlot deathSlot = base.Card.Slot;
        CardInfo cardLeft = this.ModifySpawnedCard(CardLoader.GetCardByName($"{RevenantMod.PluginGUID}_DysmorphiaLeft"));
        CardInfo cardRight = this.ModifySpawnedCard(CardLoader.GetCardByName($"{RevenantMod.PluginGUID}_DysmorphiaRight"));
        bool leftValid = leftSlot != null && leftSlot.Card == null;
        bool rightValid = rightSlot != null && rightSlot.Card == null;
        bool deathValid = deathSlot.Card == base.Card || deathSlot.Card == null; // the base card isn't unassigned 

        if (!leftValid && !rightValid && !deathValid)
            yield break;

        bool createdInDeath = false;
        if (!leftValid) // if the left slot is invalid, try to create in the death slot or in the hand
        {
            if (deathValid) // if we can create a card in the death slot
            {
                createdInDeath = true;
                yield return BoardManager.Instance.CreateCardInSlot(cardLeft, deathSlot);
            }
            else // otherwise create in the hand
            {
                RevenantMod.Log.LogInfo($"create a left card in the hand");
                yield return CardSpawner.Instance.SpawnCardToHand(cardLeft);
            }
        }
        else
            yield return BoardManager.Instance.CreateCardInSlot(cardLeft, leftSlot);

        if (!rightValid) // if the right slot is invalid, try to create in the death slot or the hand
        {
            if (deathValid && !createdInDeath) // if the death slot is valid and we haven't already filled it
                yield return BoardManager.Instance.CreateCardInSlot(cardRight, deathSlot);
            else
            {
                RevenantMod.Log.LogInfo($"create a right card in the hand"); 
                yield return CardSpawner.Instance.SpawnCardToHand(cardRight);
            }
        }
        else
            yield return BoardManager.Instance.CreateCardInSlot(cardRight, rightSlot);
    }
    public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
    {
        yield return base.PreSuccessfulTriggerSequence();
        yield return this.Effect();
        yield return base.LearnAbility(0.5f);
    }
    private CardInfo ModifySpawnedCard(CardInfo card)
    {
        List<Ability> abilities = new();
        foreach (CardModificationInfo temporaryMod in base.GetNonDefaultModsFromSelf(ability))
            abilities.AddRange(temporaryMod.abilities);
        if (abilities.Count > 4)
            abilities.RemoveRange(3, abilities.Count - 4);
        CardModificationInfo cardModificationInfo = new()
        {
            abilities = abilities,
            fromCardMerge = true
        };
        card.Mods.Add(cardModificationInfo);
        return card;
    }
}

public partial class RevenantMod
{
    public void Add_Ability_SplittingCorpse()
    {
        const string rulebookName = nameof(SplittingCorpse);
        const string rulebookDescription = "When [creature] perishes by combat, create a Dysmorphia on each empty adjacent space.";
                                         //+ "If one of the spaces is occupied, create a Dysmorphia on this card's space instead. Otherwise create a Dysmorphia in your hand.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            rulebookName.SplitPascalCase(),
            rulebookDescription,
            typeof(SplittingCorpse),
            Loader.LoadIcon(rulebookName)
            )
            .SetDefaultPart1Ability()
            .AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel2"))
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))
            ;
        ability.powerLevel = 3;
        SplittingCorpse.ability = ability.ability;
    }
}
