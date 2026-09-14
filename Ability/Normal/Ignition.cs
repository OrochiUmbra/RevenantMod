using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using Sirenix.Utilities;
using System.Collections;

namespace RevenantMod;

public class Ignition : AbilityBehaviour
{
    public static Ability ability;
    public override Ability Ability => ability;

    public const string ModSingletonId = "RevenantMod_Ignition";
    private CardModificationInfo _modInfo;

    private void Start()
    {
        _modInfo = new CardModificationInfo
        {
            singletonId = ModSingletonId
        };
        Card.AddTemporaryMod(_modInfo);
    }

    public override bool RespondsToUpkeep(bool playerUpkeep) => Card.OpponentCard != playerUpkeep && !Card.Dead;

    public override IEnumerator OnUpkeep(bool playerUpkeep)
    {
        yield return PreSuccessfulTriggerSequence();
        Card.Anim.StrongNegationEffect();
        Card.Anim.PlayTransformAnimation();
        _modInfo.attackAdjustment += 1;
        Card.Status.damageTaken += 1;
        Card.Anim.PlayHitAnimation();
        yield return LearnAbility(0.5f);

        if (Card.Health <= 0)
            yield return Card.Die(false);
    }
    /*yield return PreSuccessfulTriggerSequence();
    CardModificationInfo removeDeathtouchMod = new() { negateAbilities = { Ability.Deathtouch } };
    Singleton<ViewManager>.Instance.SwitchToView(View.Board, lockAfter: true);
    Card.Anim.StrongNegationEffect();
    Card.Anim.PlayTransformAnimation();
    Card.AddTemporaryMod(new CardModificationInfo(1, 0));
    if (Card.HasAbility(Ability.Deathtouch))
        Card.AddTemporaryMod(removeDeathtouchMod);
    yield return new WaitForSeconds(0.5f);
    yield return Card.TakeDamage(1, Card);
    if (removeDeathtouchMod != null)
        Card.RemoveTemporaryMod(removeDeathtouchMod, true);
    Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
    yield return base.LearnAbility(0.5f);*/
}

public partial class RevenantMod
{
	public void Add_Ability_Ignition()
	{
		const string rulebookName = nameof(Ignition);
		const string rulebookDescription = "[creature] gains 1 power at the cost of 1 health after each turn on the board.";

		AbilityInfo ability = AbilityManager.New(
			PluginGUID,
			rulebookName.SplitPascalCase(),
			rulebookDescription,
			typeof(Ignition),
			Loader.LoadIcon(rulebookName)
			)
			.SetDefaultPart1Ability()
			.AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel3"))
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))//.SetPixelAbilityIcon(ResourceBank.Get<Texture2D>("Art/GBC/Cards/PixelAbilityIcons/pixelability_preventattack"), FilterMode.Point)
            ;
		ability.powerLevel = 0;
		Ignition.ability = ability.ability;
		/*
		AbilityBuilder<Ignition>.Builder
		.SetIcon(Asset.LoadIcon(rulebookName))
		.SetRulebookDescription(rulebookDescription)
		.Build();
		*/
		/*
		var ability = AbilityManager.Add(PluginGuid, new AbilityInfo() {
		rulebookName = "Ignition",
		rulebookDescription = "[creature] gains +1 power with the cost of -1 health after each turn on the board.",
		metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part1Rulebook } },
		typeof(Ignition),
		TextureHelper.GetImageAsTexture("ability_Ignition.png"));

		Ignition.ability = ability.Id;
		*/
	}
	/*private void AddCaustic()
	{
		AbilityInfo caustic = AbilityManager.New(
		RevenantMod.PluginGuid + ".causticability",
		"Caustic",
		"Any attack [creature] performs will apply a Damage over time effect to the Card hit.",
		 typeof(CausticAbility),
		 "ability_caustic.png"
		)
		 .SetDefaultPart1Ability()
		 .SetPixelAbilityIcon(TextureHelper.GetImageAsTexture("pixel_caustic.png"), FilterMode.Point);

		CausticAbility.ability = caustic.ability;
		caustic.powerLevel = 4;
	}*/
}
/*public class CausticAbility : AbilityBehaviour
{

    public override Ability Ability
    {
        get
        {
            return ability;
        }
    }
    public override bool RespondsToDealDamage(int amount, PlayableCard target)
    {
        if (target.Dead)
        {
            return false;
        }
        return base.Card.HasAbility(CausticAbility.ability);
    }

    public override IEnumerator OnDealDamage(int amount, PlayableCard target)
    {
        if (target != null)
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
            yield return new WaitForSeconds(0.1f);
            base.Card.Anim.LightNegationEffect();
            yield return base.PreSuccessfulTriggerSequence();
            CardModificationInfo cardModificationInfo = new CardModificationInfo(DotAbility.ability);
            CardInfo targetCardInfo = target.Info.Clone() as CardInfo;
            targetCardInfo.Mods.Add(cardModificationInfo);
            target.SetInfo(targetCardInfo);
            target.Anim.PlayTransformAnimation();
            yield return new WaitForSeconds(0.1f);
            yield return base.LearnAbility(0.1f);
            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
        }
        yield break;
    }


    public static Ability ability;
}*/
