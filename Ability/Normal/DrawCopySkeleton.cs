using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using System.Collections;
using System.Collections.Generic;

namespace RevenantMod;

public class DrawCopySkeleton : DrawCreatedCard
{
    public static Ability ability;
    public override Ability Ability => ability;

    public override CardInfo CardToDraw
    {
        get
        {
            // Creates a skeleton that has the abilities, tribes, power of the killed card
            CardInfo skeleton = CardLoader.GetCardByName("Skeleton");

            skeleton.displayedName = base.Card.Info.displayedName;
            skeleton.appearanceBehaviour = base.Card.Info.appearanceBehaviour;

            foreach (Ability item in base.Card.Info.DefaultAbilities.FindAll(x => !base.Card.Status.hiddenAbilities.Contains(x)))
                // Adds base sigils
                skeleton.Mods.Add(new CardModificationInfo(item)/* { negateAbilities = { ability } }*/);
            foreach (CardModificationInfo item in base.GetNonDefaultModsFromSelf(ability)/*base.Card.Info.Mods.FindAll((CardModificationInfo x) => !x.nonCopyable)*/)
            {
                // Adds merged sigils
                CardModificationInfo cardModificationInfo = (CardModificationInfo)item.Clone();
                //if (cardModificationInfo.healthAdjustment > 0)
                //    cardModificationInfo.healthAdjustment = 0;
                skeleton.Mods.Add(cardModificationInfo);
            }
            foreach (Tribe item in base.Card.Info.tribes.FindAll((Tribe x) => x != Tribe.NUM_TRIBES))
                // Adds tribes
                skeleton.tribes.Add(item);
            return skeleton;
        }
    }

    public override List<CardModificationInfo> CardToDrawTempMods => new() { new CardModificationInfo() { negateAbilities = { ability } } };

    public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer) => true;

    public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
    {
        yield return base.PreSuccessfulTriggerSequence();
        base.Card.Anim.StrongNegationEffect();
        yield return base.CreateDrawnCard();
        yield return base.LearnAbility(0.5f);
    }
    /*public override bool RespondsToPreDeathAnimation(bool wasSacrifice) => Card.OnBoard;

    public override IEnumerator OnPreDeathAnimation(bool wasSacrifice)
    {
        CardInfo info = CardLoader.GetCardByName("Skeleton");
        List<CardModificationInfo> mods = base.GetNonDefaultModsFromSelf(this.Ability);
        foreach (CardModificationInfo mod in mods)
        {
            mod.fromCardMerge = false;
        }
        info.Mods.AddRange(mods);
        if (ViewManager.Instance.CurrentView != View.Default)
        {
            yield return new WaitForSeconds(0.25f);
            ViewManager.Instance.SwitchToView(View.Default);
        }
        yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(info);
        yield return base.LearnAbility(0.5f);
    }*/
}

public partial class RevenantMod
{
    public void Add_Ability_DrawCopySkeleton()
    {
        const string rulebookName = nameof(DrawCopySkeleton);
        const string rulebookDescription = "When [creature] perishes, a Skeleton with its old sigils enters your hand. "
                                         + "A Skeleton is defined as: 1 Power, 1 Health, Brittle.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            "Skeletonize",
            rulebookDescription,
            typeof(DrawCopySkeleton),
            Loader.LoadIcon(rulebookName)
            )
            .SetDefaultPart1Ability()
            .AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel1"))
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))
            ;
        ability.powerLevel = 2;
        DrawCopySkeleton.ability = ability.ability;
    }
}
