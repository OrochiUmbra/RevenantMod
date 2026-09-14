using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using InscryptionAPI.Helpers;
using Sirenix.Utilities;
using System.Collections;
using UnityEngine;

namespace RevenantMod;

public class ActivatedBonesToEnergy : ActivatedAbilityBehaviour
{
    public static Ability ability;
    public override Ability Ability => ability;
    
    private const int BONES_COST = 1;
    public override int BonesCost => BONES_COST;

    private int active = 0; // this doesn't need to be static

    public override bool CanActivate() => Singleton<ResourcesManager>.Instance.PlayerEnergy < 6;

    public override IEnumerator Activate()
    {
        yield return base.PreSuccessfulTriggerSequence();
        base.Card.Anim.StrongNegationEffect();
        AudioController.Instance.PlaySound2D("creepy_cue_long_lower", MixerGroup.GBCSFX, 1f, 0.625f, null, null, null, null, false);
        active++; // active should be counted once conditions are made
        //if (!Singleton<ResourcesManager>.Instance.EnergyAtMax) // revisit code for 6+ energy support
        yield return Singleton<ResourcesManager>.Instance.AddMaxEnergy(1);
        yield return Singleton<ResourcesManager>.Instance.AddEnergy(1);
        yield return base.LearnAbility();
    }
    
    // since this is an activated ability, we only want OnTurnEnd to trigger when it's the player's turn's end
    public override bool RespondsToTurnEnd(bool playerTurnEnd) => active > 0 && !base.Card.OpponentCard && playerTurnEnd && !Card.Dead;
    public override IEnumerator OnTurnEnd(bool playerTurnEnd)
    {
        RevenantMod.Log.LogInfo($"[Energy Rebound] Turn End: {Singleton<ResourcesManager>.Instance.PlayerMaxEnergy} {Singleton<ResourcesManager>.Instance.PlayerEnergy}");
        base.Card.Anim.StrongNegationEffect();
        int rebound = Mathf.Min(Singleton<ResourcesManager>.Instance.PlayerMaxEnergy, active);
        Singleton<ResourcesManager>.Instance.PlayerEnergy = Mathf.Max(Singleton<ResourcesManager>.Instance.PlayerEnergy - rebound, 0);
        yield return Singleton<ResourcesManager>.Instance.ShowSpendEnergy(rebound);
        Singleton<ResourcesManager>.Instance.PlayerMaxEnergy = Mathf.Max(Singleton<ResourcesManager>.Instance.PlayerMaxEnergy - rebound, 0);
        Singleton<ResourcesManager>.Instance.PlayerEnergy = Mathf.Min(Singleton<ResourcesManager>.Instance.PlayerEnergy, Singleton<ResourcesManager>.Instance.PlayerMaxEnergy);
        yield return Singleton<ResourcesManager>.Instance.ShowRemoveMaxEnergy(rebound);
        //yield return OnDie(false, null);
        active = 0;
        RevenantMod.Log.LogInfo($"[Energy Rebound] Turn End2: {Singleton<ResourcesManager>.Instance.PlayerMaxEnergy} {Singleton<ResourcesManager>.Instance.PlayerEnergy}");
    }

    public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer) => active > 0 && !Card.OpponentCard;

    public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
    {
        RevenantMod.Log.LogInfo("[Energy Rebound] Die");
        // since OnDie just removes energy, we can call OnTurnEnd here instead of copying code
        yield return OnTurnEnd(!Card.OpponentCard);
    }
    /*
    public override bool RespondsToUpkeep(bool playerUpkeep) => active > 0 && Card.OpponentCard != playerUpkeep;

    public override IEnumerator OnUpkeep(bool playerUpkeep)
    {
        base.Card.Anim.StrongNegationEffect();
        yield return Singleton<ResourcesManager>.Instance.SpendEnergy(Math.Min(active, Singleton<ResourcesManager>.Instance.PlayerEnergy));
        yield return Singleton<ResourcesManager>.Instance.RemoveMaxEnergy(Math.Min(active, Singleton<ResourcesManager>.Instance.PlayerMaxEnergy));
        active = 0;
    }
    */
}

public partial class RevenantMod
{
    public void Add_Ability_ActivatedBonesToEnergy()
    {
        const string rulebookName = "Soulgate";
        const string rulebookDescription = "Pay 1 Bone to gain 1 Energy, lose as much Max Energy at the end of the owner's turn.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            rulebookName.SplitPascalCase(),
            rulebookDescription,
            typeof(ActivatedBonesToEnergy),
            Loader.LoadIcon(rulebookName)
            )
            .SetDefaultPart1Ability()
            .SetActivated()
            .AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel1"))
            //.SetPixelAbilityIcon(Asset.LoadTexture("pixelability_activated_" + rulebookName), FilterMode.Point)
            ;
        ability.powerLevel = 3;
        ability.pixelIcon = Loader.LoadSprite("pixelability_activated_" + rulebookName);
        ActivatedBonesToEnergy.ability = ability.ability;
    }
}
