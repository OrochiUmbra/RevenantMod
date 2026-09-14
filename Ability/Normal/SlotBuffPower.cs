using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RevenantMod;

[HarmonyPatch]
public class SlotBuffPower : AbilityBehaviour
{
    public static Ability ability;
    public override Ability Ability => ability;

    internal static List<CardSlot> BuffedSlots = new();

    public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
    {
        PowerUpSlot(this.Card.Slot);
        yield break;
    }

    public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer) => true;

    [HarmonyPatch(typeof(PlayableCard), nameof(PlayableCard.GetPassiveAttackBuffs))]
    [HarmonyPostfix]
    private static void AddBuffForCardSlot(PlayableCard __instance, ref int __result)
    {
        if (BuffedSlots.Contains(__instance.Slot))
            __result += 1;
    }

    [HarmonyPatch(typeof(TurnManager), nameof(TurnManager.SetupPhase))]
    [HarmonyPostfix]
    private static void ResetBuffedSlots()
    {
        BuffedSlots.Clear();
    }

    [HarmonyPatch(typeof(TurnManager), nameof(TurnManager.CleanupPhase))]
    [HarmonyPostfix]
    private static void CleanUpBuffedSlots()
    {
        foreach (CardSlot slot in BuffedSlots)
            DepowerSlot(slot);
        BuffedSlots.Clear();
    }

    private static void PowerUpSlot(CardSlot slot)
    {
        BuffedSlots.Add(slot);
        slot.SetTexture(Loader.LoadTexture("cardslot_buffpower"));
    }

    private static void DepowerSlot(CardSlot slot)
    {
        if (SaveManager.SaveFile.IsPart1)
            slot.SetTexture(ResourceBank.Get<Texture>("Art/Cards/card_slot"));
        if (SaveManager.SaveFile.IsPart3)
            slot.SetTexture(ResourceBank.Get<Texture>("Art/Cards/card_slot_tech"));
        if (SaveManager.SaveFile.IsGrimora)
            slot.SetTexture(ResourceBank.Get<Texture>("Art/Cards/card_slot_undead"));
        if (SaveManager.SaveFile.IsMagnificus)
            slot.SetTexture(ResourceBank.Get<Texture>("Art/Cards/card_slot_wizard"));
    }
}

public partial class RevenantMod
{
    public void Add_Ability_SlotBuffPowerTakeHealth()
    {
        const string rulebookName = "Rancor";
        const string rulebookDescription = "When [creature] perishes, it leaves a permanent 1 power buff in the space it occupied.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            rulebookName,
            rulebookDescription,
            typeof(SlotBuffPower),
            Loader.LoadIcon(rulebookName)
            )
            .SetDefaultPart1Ability()
            .AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel2"), AbilityMetaCategory.Part1Rulebook)
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))
            ;
        ability.powerLevel = 2;
        SlotBuffPower.ability = ability.ability;
    }
}

/*
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RevenantMod;

public class SlotBuffPowerTakeHealth : AbilityBehaviour
{
    public static Ability ability;
    public override Ability Ability => ability;

    public override bool RespondsToPreDeathAnimation(bool wasSacrifice) => true;

    public override IEnumerator OnPreDeathAnimation(bool wasSacrifice)
    {
        yield return PreSuccessfulTriggerSequence();
        HauntedSlot.SetupSlot(Card);
        yield return base.LearnAbility(0.5f);
    }
}
//Grimora Haunter Port
public class HauntedSlot : NonCardTriggerReceiver
{
    [SerializeField]
    public CardSlot cardSlot;

    [SerializeField]
    public List<Ability> abilities = new();

    private CardModificationInfo _modInfo;

    public static HauntedSlot SetupSlot(PlayableCard playableCard)
    {
        Material defaultIconMat = playableCard.AbilityIcons.defaultIconMat;
        HauntedSlot hauntedSlot = new GameObject(playableCard.Slot.name + "_Haunted").AddComponent<HauntedSlot>();
        hauntedSlot.transform.SetParent(playableCard.Slot.transform);
        hauntedSlot.cardSlot = playableCard.Slot;
        hauntedSlot.abilities = playableCard.AbilityIcons.abilityIcons
          .Where(abilityIconInteractable => abilityIconInteractable.Ability != GuidManager.GetEnumValue<Ability>("arackulele.inscryption.grimoramod", "Haunter"))
          .ForEach(icon =>
          {
              AbilityIconInteractable hauntedIcon = Instantiate(
                 icon,
                 icon.transform.position + new Vector3(0, 0.1f),
                 Quaternion.Euler(90, 0, 0),
                 hauntedSlot.transform
             );
              hauntedIcon.gameObject.layer = icon.gameObject.layer;
              hauntedIcon.name = AbilityManager.AllAbilities.Find(fa => fa.Id == icon.Ability).Info.rulebookName;
              Renderer renderer = hauntedIcon.GetComponent<MeshRenderer>();
              hauntedIcon.GetComponent<Renderer>().material.SetColor("_Color", GameColors.instance.yellow);

              //TODO: fix it for community patch
              if (hauntedIcon.GetComponent<InscryptionCommunityPatch.Card.ActivatedAbilityIconInteractable>() != null) UnityEngine.Object.Destroy(hauntedIcon.GetComponent<InscryptionCommunityPatch.Card.ActivatedAbilityIconInteractable>());
              if (hauntedIcon.GetComponent<AbilityIconInteractable>() != null) UnityEngine.Object.Destroy(hauntedIcon.GetComponent<AbilityIconInteractable>());
              renderer.enabled = true;
              // renderer.material.ChangeRenderMode(UnityObjectExtensions.BlendMode.Cutout);
              // this is so that it doesn't appear behind the card slot texture at the lowest point in the wave movement 
              // renderer.sortingOrder = 1;
              hauntedIcon.SetIcon(icon.GetComponent<Renderer>().material.mainTexture);

              SineWaveMovement wave = hauntedIcon.gameObject.AddComponent<SineWaveMovement>();
              wave.speed = 1;
              wave.xMagnitude = 0;
              wave.yMagnitude = 0.1f;
              wave.zMagnitude = 0;

              hauntedSlot.abilities.Add(icon.Ability);
          })
          .Select(icon => icon.Ability)
          .ToList();

        hauntedSlot._modInfo = new CardModificationInfo
        {
            abilities = new List<Ability>(hauntedSlot.abilities)
        };

        return hauntedSlot;
    }

    public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
    {
        return otherCard.Slot == cardSlot;
    }

    public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
    {
        int maxCount = (GrimoraModHelpers.GrimoraModIsActive() ? 5 : 8);
        List<Ability> cardAllAbilities = CardExtensions.AllAbilities(otherCard);
        if (cardAllAbilities.Count == maxCount)
        {
            yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("Oh... it will be rather difficult to haunt " + "[c:lGr]" + otherCard.Info.DisplayedNameEnglish + "[c:]" + " with their abilities at max capacity.");
        }
        else
        {
            if (cardAllAbilities.Count + _modInfo.abilities.Count > maxCount)
            {
                List<Ability> abilitiesToAdd = _modInfo.abilities.GetRange(0, maxCount);
                _modInfo.abilities = abilitiesToAdd;
            }

            otherCard.AddTemporaryMod(_modInfo);
            otherCard.Anim.PlayTransformAnimation();
            otherCard.UpdateStatsText();
            otherCard.RenderCard();
            yield return new WaitForSeconds(0.1f);
        }
        RevenantMod.Log.LogInfo($"Destroying HauntedSlot [{this}]");
        CustomCoroutine.WaitThenExecute(0.1f, delegate
        {
            Object.Destroy(base.gameObject);
        });
    }
}*/
