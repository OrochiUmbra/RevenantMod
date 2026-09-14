using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RevenantMod;

public class LatchGrimoraRandomPrime : Latch
{
    public static Ability ability;
    public override Ability Ability => ability;
    public override Ability LatchAbility => ability;

    public override bool RespondsToTurnEnd(bool playerTurnEnd) => !Card.Status.hiddenAbilities.Contains(Ability);

    public override IEnumerator OnTurnEnd(bool playerTurnEnd)
    {
        yield return PreSuccessfulTriggerSequence();
        Card.Anim.PlayTransformAnimation();
        AddMod();
        yield return new WaitForSeconds(0.5f);
        yield return LearnAbility(0.5f);
    }
    private void AddMod()
    {
        Card.Status.hiddenAbilities.Add(Ability);
        CardModificationInfo cardModificationInfo = new CardModificationInfo(ChooseAbility());
        CardModificationInfo cardModificationInfo2 = Card.TemporaryMods.Find(x => x.HasAbility(Ability))
                                                  ?? Card.Info.Mods.Find(x => x.HasAbility(Ability));

        if (cardModificationInfo2 != null)
        {
            cardModificationInfo.fromTotem = cardModificationInfo2.fromTotem;
            cardModificationInfo.fromCardMerge = cardModificationInfo2.fromCardMerge;
        }

        Card.AddTemporaryMod(cardModificationInfo);
    }

    private Ability ChooseAbility()
    {
        List<Ability> defaultAbilities = GetAbilityList(SaveManager.SaveFile.IsPart1 ? AbilityMetaCategory.Part1Modular : AbilityMetaCategory.Part3Modular);
        //List<Ability> grimoraAbilities = GetRandomList(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel" + Random.Range(1, 3).ToString()));

        Ability randomAbility = Ability.RandomAbility;
        int num2 = 0;
        while (randomAbility == Ability.RandomAbility)
        {
            randomAbility = GrimoraModHelpers.GrimoraModIsActive() ? GrimoraModHelpers.GetRandomElectricAbility() : defaultAbilities[Random.Range(0, defaultAbilities.Count)];
            //ElectricChairLever.AbilitiesMajorRisk.GetRandomItem()
            num2++;
            if (num2 > 100)
                return Ability.Sharp;
        }

        RevenantMod.Log.LogInfo($"[LatchGrimoraRandomPrime] Random ability chosen [{randomAbility}]");
        return randomAbility;
    }

    private List<Ability> GetAbilityList(AbilityMetaCategory metaCategory)
    {
        List<Ability> learnedAbilities = AbilitiesUtil.GetLearnedAbilities(false, 0, 5, metaCategory);
        learnedAbilities.RemoveAll((Ability x) => x == Ability.RandomAbility || base.Card.HasAbility(x));
        return learnedAbilities;
    }
}

[HarmonyPatch(typeof(RandomAbility), "ChooseAbility")]
internal class RevRandomAbilityPatch
{
    [HarmonyPostfix]
    public static void GetChairAbilityPatch(ref Ability __result)
    {
        if (GrimoraModHelpers.GrimoraModIsActive())
        {
            __result = GrimoraModHelpers.GetRandomElectricAbility();
            RevenantMod.Log.LogInfo($"[RevRandomAbilityPatch] Random ability chosen [{__result}]");
        }
    }
}

public partial class RevenantMod
{
    public void Add_Ability_LatchGrimoraRandom()
    {
        const string rulebookName = nameof(LatchGrimoraRandomPrime);
        const string rulebookDescription = "At the end of the owner's turn, this sigil is replaced with another sigil at random. "
                                         + "When it perishes, its owner chooses a creature to gain this sigil.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            "Skull Storm",
            rulebookDescription,
            typeof(LatchGrimoraRandomPrime),
            Loader.LoadIcon(rulebookName)
            )
            .SetDefaultPart1Ability()
            .AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel3"))
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))
            ;
        ability.powerLevel = 4;
        LatchGrimoraRandomPrime.ability = ability.ability;
    }
}
