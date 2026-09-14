using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using Sirenix.Utilities;
using System.Collections;
using UnityEngine;

namespace RevenantMod;
// NevernamedsSigils Twister Base
public class ActivatedTransform : ActivatedAbilityBehaviour
{
    public static Ability ability;
    public override Ability Ability => ability;
    
    private const int BONES_COST = 2;
    public override int BonesCost => BONES_COST;

    bool isCurrentlyStatTransformed;
    public CardModificationInfo statTransformation;
    public CardInfo cardCopy;

    public override IEnumerator Activate()
    {
        yield return base.PreSuccessfulTriggerSequence();
        if (!base.Card.Dead)
        {
            if (Card.Info.GetExtendedProperty("TransmuteForm") != null)
            {
                CardInfo target = CardLoader.GetCardByName(Card.Info.GetExtendedProperty("TransmuteForm"));
                foreach (CardModificationInfo mod in base.Card.Info.Mods.FindAll((CardModificationInfo x) => !x.nonCopyable))
                {
                    CardModificationInfo clone = (CardModificationInfo)mod.Clone();
                    if (target.HasAbility(ability) && clone.HasAbility(ability))
                    {
                        clone.abilities.Remove(ability);
                    }
                    target.Mods.Add(clone);
                }
                yield return base.PreSuccessfulTriggerSequence();
                yield return base.Card.TransformIntoCard(target);
                yield return new WaitForSeconds(0.3f);
            }
            else //Default Banshee transformation
            {
                CardInfo fallBack = CardLoader.GetCardByName("Banshee");
                foreach (CardModificationInfo mod in base.Card.Info.Mods.FindAll((CardModificationInfo x) => !x.nonCopyable))
                {
                    CardModificationInfo clone = (CardModificationInfo)mod.Clone();
                    if (fallBack.HasAbility(ability) && clone.HasAbility(ability))
                    {
                        clone.abilities.Remove(ability);
                    }
                    fallBack.Mods.Add(clone);
                }

                if (cardCopy == null)
                {
                    cardCopy = base.Card.Info.Clone() as CardInfo;
                }
                foreach (CardModificationInfo mod in base.Card.Info.Mods.FindAll((CardModificationInfo x) => !x.nonCopyable))
                {
                    CardModificationInfo clone = (CardModificationInfo)mod.Clone();
                    if (cardCopy.HasAbility(ability) && clone.HasAbility(ability))
                    {
                        clone.abilities.Remove(ability);
                    }
                    cardCopy.Mods.Add(clone);
                }

                if (statTransformation == null)
                {
                    statTransformation = new CardModificationInfo(base.Card.Attack, base.Card.Health);
                    statTransformation.nameReplacement = "Transmuted " + base.Card.Info.DisplayedNameLocalized;
                }
                if (isCurrentlyStatTransformed)
                {
                    Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                    yield return new WaitForSeconds(0.15f);
                    yield return base.Card.TransformIntoCard(cardCopy);
                    yield return new WaitForSeconds(0.15f);
                    base.Card.RemoveTemporaryMod(statTransformation);
                    base.Card.RenderCard();
                    isCurrentlyStatTransformed = false;
                }
                else
                {
                    Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                    yield return new WaitForSeconds(0.15f);
                    yield return base.Card.TransformIntoCard(fallBack);
                    yield return new WaitForSeconds(0.15f);
                    base.Card.AddTemporaryMod(statTransformation);
                    base.Card.RenderCard();
                    isCurrentlyStatTransformed = true;
                }
            }
            if (base.Card.Health <= 0)
            {
                yield return base.Card.Die(false);
            }
            yield return base.LearnAbility(0.5f);
        }
    }
}

public partial class RevenantMod
{
    public void Add_Ability_ActivatedTransform()
    {
        const string rulebookName = "Transmute";
        const string rulebookDescription = "Pay 2 Bones to transform [creature] to its alternate form.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            rulebookName.SplitPascalCase(),
            rulebookDescription,
            typeof(ActivatedTransform),
            Loader.LoadIcon(rulebookName)
            )
            .SetDefaultPart1Ability()
            .SetActivated()
            .AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel3"))
            //.SetPixelAbilityIcon(Asset.LoadTexture("pixelability_activated_" + rulebookName), FilterMode.Point)
            ;
        ability.powerLevel = 1;
        ability.pixelIcon = Loader.LoadSprite("pixelability_activated_" + rulebookName);
        ActivatedTransform.ability = ability.ability;
    }
}
