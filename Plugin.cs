using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using DiskCardGame;
using GrimoraMod;
using HarmonyLib;
using Infiniscryption.PackManagement;
using InscryptionAPI;
using InscryptionAPI.Ascension;
using InscryptionAPI.Guid;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RevenantMod;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
[BepInDependency(InscryptionAPIPlugin.ModGUID, BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency("zorro.inscryption.infiniscryption.packmanager", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("zorro.inscryption.infiniscryption.p03kayceerun", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("arackulele.inscryption.grimoramod", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("kopie.inscryption.hammermod", BepInDependency.DependencyFlags.SoftDependency)]

[HarmonyPatch]
public partial class RevenantMod : BaseUnityPlugin
{
	internal static readonly Harmony harmony = new(PluginGUID);
    public const string PluginGUID = "orochiumbra.inscryption.revenantmod";
    public const string PluginName = "RevenantMod";
    public const string PluginVersion = "0.0.20";
	public static ManualLogSource Log;
	public static readonly CardMetaCategory GBCPack = CardMetaCategory.GBCPack;
    public static readonly CardMetaCategory GBCPlayable = CardMetaCategory.GBCPlayable;
    public static readonly CardMetaCategory GrimoraChoice = GuidManager.GetEnumValue<CardMetaCategory>("arackulele.inscryption.grimoramod", "GrimoraModChoiceNode");
	public static readonly CardMetaCategory WizardRegion = GuidManager.GetEnumValue<CardMetaCategory>("zorro.inscryption.infiniscryption.p03kayceerun", "WizardRegionCards");
	public static readonly CardMetaCategory NatureRegion = GuidManager.GetEnumValue<CardMetaCategory>("zorro.inscryption.infiniscryption.p03kayceerun", "NatureRegionCards");
	public static readonly CardMetaCategory UndeadRegion = GuidManager.GetEnumValue<CardMetaCategory>("zorro.inscryption.infiniscryption.p03kayceerun", "UndeadRegionCards");
	public static readonly CardMetaCategory TechRegion = GuidManager.GetEnumValue<CardMetaCategory>("zorro.inscryption.infiniscryption.p03kayceerun", "TechRegionCards");
	public static readonly CardMetaCategory NeutralRegion = GuidManager.GetEnumValue<CardMetaCategory>("zorro.inscryption.infiniscryption.p03kayceerun", "NeutralRegionCards");
    public static readonly CardTemple DynamicCardTemple = Chainloader.PluginInfos.ContainsKey("zorro.inscryption.infiniscryption.p03kayceerun") ? CardTemple.Tech : (Chainloader.PluginInfos.ContainsKey("arackulele.inscryption.grimoramod") ? CardTemple.Undead : CardTemple.Nature);
	//public static CardTemple Part2CardTemple = CardTemple.Undead;
	//public static CardTemple CardTemple => SaveManager.SaveFile.IsPart2 ? Part2CardTemple : DynamicCardTemple;
    /*public static CardTemple ScreenState
    {
        get
        {
            string value = ModdedSaveManager.SaveData.GetValue(PluginGuid, "ScreenState");
            // Log.LogDebug($"[CardTemple.ScreenState.Getter] Value is [{value}]");
            return string.IsNullOrEmpty(value) ? CardTemple.Nature : (CardTemple)Enum.Parse(typeof(CardTemple), value);
        }
        set => ModdedSaveManager.SaveData.SetValue(PluginGuid, "ScreenState", value);
    }*/
    private void Awake()
	{
		Log = base.Logger;
		harmony.PatchAll(Assembly.GetExecutingAssembly());
		LoadAbility();
		LoadCard();
        //LoadBloodCard();
		HammerStrikePatch.PatchHammer();
        Log.LogInfo($"{PluginName} Loaded.");
        CreateStarterdecks();
        Log.LogInfo($"{PluginName} Stock Starter Decks Loaded.");
        if (Chainloader.PluginInfos.ContainsKey("arackulele.inscryption.grimoramod"))
		{
            CreateGrimoraStarterdecks();
            Log.LogInfo($"{PluginName} Grimora Starter Decks Loaded.");
            ManageStarterdecks();
            HammerStrikePatchGrimora.PatchHammer();
            Log.LogInfo($"{PluginName} Grimora Hammer Patched.");
        }
        if (Chainloader.PluginInfos.ContainsKey("kopie.inscryption.hammermod"))
        {
            HammerStrikePatchKopie.PatchHammer();
            Log.LogInfo($"{PluginName} Smalltweak Hammer Patched.");
        }
        if (Chainloader.PluginInfos.ContainsKey("zorro.inscryption.infiniscryption.packmanager"))
		{
            CreatePack();
            Log.LogInfo($"{PluginName} Card Pack Loaded.");
        }
    }
	private void LoadAbility()
	{
		AccessTools.GetDeclaredMethods(typeof(RevenantMod))
		.Where(mi => mi.Name.StartsWith("Add_Ability"))
		.ForEach(mi => mi.Invoke(this, null));
	}
	private void LoadCard()
	{
		AccessTools.GetDeclaredMethods(typeof(RevenantMod))
		.Where(mi => mi.Name.StartsWith("Add_Card"))
		.ForEach(mi => mi.Invoke(this, null));
	}
	private void LoadBloodCard()
	{
		AccessTools.GetDeclaredMethods(typeof(RevenantMod))
		.Where(mi => mi.Name.StartsWith("Add_Blood_Card"))
		.ForEach(mi => mi.Invoke(this, null));
	}
	private void CreateStarterdecks()
	{
		StarterDeckManager.New(PluginGUID, "StockDeck_Soulgate", Loader.LoadTexture("deck_soulgate"), new string[]
		{
			$"{PluginGUID}_Soulgate",
			$"{PluginGUID}_Allip",
			$"{PluginGUID}_MoleDigger"
		});

		StarterDeckManager.New(PluginGUID, "StockDeck_Latcher", Loader.LoadTexture("deck_latcher"), new string[]
		{
			$"{PluginGUID}_RavenStillborn",
			$"{PluginGUID}_UndeadCivilian",
			$"{PluginGUID}_Undertaker"
		});

        StarterDeckManager.New(PluginGUID, "StockDeck_Digger", Loader.LoadTexture("deck_digger"), new string[]
        {
            $"{PluginGUID}_SpadeWarrior",
            $"{PluginGUID}_Undertaker",
            $"{PluginGUID}_MoleDigger"
        });

        StarterDeckManager.New(PluginGUID, "StockDeck_UndeadBeast", Loader.LoadTexture("deck_undeadbeast"), new string[]
		{
			$"{PluginGUID}_Ourobones",
			$"{PluginGUID}_DeadStoat",
			$"{PluginGUID}_MoleDigger"
		});

		StarterDeckManager.New(PluginGUID, "StockDeck_TestSubject", Loader.LoadTexture("deck_testsubject"), new string[]
		{
			$"{PluginGUID}_Limbs",
			$"{PluginGUID}_OdiousMass",
			$"{PluginGUID}_BrainJar"
		});

		StarterDeckManager.New(PluginGUID, "StockDeck_Necrosis", Loader.LoadTexture("deck_necrosis"), new string[]
		{
			$"{PluginGUID}_NekrosPrime",
			$"{PluginGUID}_Nekros",
			$"{PluginGUID}_TheBloated"
		});

		StarterDeckManager.New(PluginGUID, "StockDeck_Calamity", Loader.LoadTexture("deck_calamity"), new string[]
		{
			$"{PluginGUID}_InochiPrime",
			$"{PluginGUID}_Ankhame",
			$"{PluginGUID}_Soulgate"
		});

		StarterDeckManager.New(PluginGUID, "StockDeck_Delta", Loader.LoadTexture("deck_delta"), new string[]
		{
			$"{PluginGUID}_Sans",
			$"{PluginGUID}_Ralsei",
			$"{PluginGUID}_Skelephone"
		});

		StarterDeckManager.New(PluginGUID, "StockDeck_Creator", Loader.LoadTexture("deck_creator"), new string[]
		{
			$"{PluginGUID}_AnneBean",
			$"{PluginGUID}_WhistleWind",
			$"{PluginGUID}_OrochiUmbra"
		});

		StarterDeckManager.FullStarterDeck ritual = StarterDeckManager.New(PluginGUID, "StockDeck_Ritual", Loader.LoadTexture("deck_ritual"), new string[]
		{
			$"{PluginGUID}_Combustor",
			$"{PluginGUID}_UndeadCivilian",
			$"{PluginGUID}_UndeadVillage",
			$"{PluginGUID}_Undertaker",
            "Smoke"
        });
		ritual.CustomUnlockCheck = dummy => RevenantSaveManager.UnlockRitualDeck;
	}
	private void CreateGrimoraStarterdecks()
	{
		StarterDecks.AddGrimoraStarterdeck(new List<string>()
		{
			$"{PluginGUID}_Soulgate",
			$"{PluginGUID}_Allip",
			$"{PluginGUID}_MoleDigger",
			$"{PluginGUID}_SteinNFrank",
            "Revenant"
        },
		Loader.LoadSprite("deck_soulgate"), "GrimoraDeck_Soulgate");

		StarterDecks.AddGrimoraStarterdeck(new List<string>()
		{
            $"{PluginGUID}_RavenStillborn",
            $"{PluginGUID}_UndeadCivilian",
            $"{PluginGUID}_Undertaker",
			GrimoraPlugin.NameHellhand,
			$"{PluginGUID}_FulurOssa"
        },
		Loader.LoadSprite("deck_latcher"), "GrimoraDeck_Latcher");

        StarterDecks.AddGrimoraStarterdeck(new List<string>()
        {
            $"{PluginGUID}_SpadeWarrior",
            $"{PluginGUID}_Undertaker",
            $"{PluginGUID}_MoleDigger",
            GrimoraPlugin.NameGravedigger,
            GrimoraPlugin.NameBonepile
        },
        Loader.LoadSprite("deck_digger"), "GrimoraDeck_Digger");

        StarterDecks.AddGrimoraStarterdeck(new List<string>()
		{
			$"{PluginGUID}_Ourobones",
			$"{PluginGUID}_DeadStoat",
			$"{PluginGUID}_MoleDigger",
			$"{PluginGUID}_Wendigo",
			GrimoraPlugin.NameBonehound
		},
		Loader.LoadSprite("deck_undeadbeast"), "GrimoraDeck_UndeadBeast");

		StarterDecks.AddGrimoraStarterdeck(new List<string>()
		{
			$"{PluginGUID}_Limbs",
			$"{PluginGUID}_OdiousMass",
			$"{PluginGUID}_BrainJar",
			$"{PluginGUID}_SteinNFrank",
			GrimoraPlugin.NameApparition
		},
		Loader.LoadSprite("deck_testsubject"), "GrimoraDeck_TestSubject");

		StarterDecks.AddGrimoraStarterdeck(new List<string>()
		{
			$"{PluginGUID}_NekrosPrime",
			$"{PluginGUID}_Nekros",
			$"{PluginGUID}_TheBloated",
			$"{PluginGUID}_Anorexia",
			GrimoraPlugin.NameNecromancer
		},
		Loader.LoadSprite("deck_necrosis"), "GrimoraDeck_Necrosis");

		StarterDecks.AddGrimoraStarterdeck(new List<string>()
		{
            $"{PluginGUID}_InochiPrime",
            $"{PluginGUID}_Ankhame",
            $"{PluginGUID}_Soulgate",
            $"{PluginGUID}_Harbinger",
            GrimoraPlugin.NameGravedigger,
        },
		Loader.LoadSprite("deck_calamity"), "GrimoraDeck_Calamity");

		StarterDecks.AddGrimoraStarterdeck(new List<string>()
		{
			$"{PluginGUID}_Sans",
			$"{PluginGUID}_Ralsei",
			$"{PluginGUID}_Skelephone",
			GrimoraPlugin.NameDoll,
			GrimoraPlugin.NameDeadHand
		},
		Loader.LoadSprite("deck_delta"), "GrimoraDeck_Delta");

		StarterDecks.AddGrimoraStarterdeck(new List<string>()
		{
			$"{PluginGUID}_AnneBean",
			$"{PluginGUID}_WhistleWind",
			$"{PluginGUID}_OrochiUmbra",
			$"{PluginGUID}_InochiPrime",
			GrimoraPlugin.NameOurobones
		},
		Loader.LoadSprite("deck_creator"), "GrimoraDeck_Creator");

		StarterDecks.AddGrimoraStarterdeck(new List<string>()
		{
			$"{PluginGUID}_Combustor",
			$"{PluginGUID}_UndeadCivilian",
			$"{PluginGUID}_UndeadVillage",
			$"{PluginGUID}_Undertaker",
			GrimoraPlugin.NameBonepile
		},
		Loader.LoadSprite("deck_ritual"), "GrimoraDeck_Ritual");
	}
	private void ManageStarterdecks()
	{
		StarterDeckManager.ModifyDeckList += delegate (List<StarterDeckManager.FullStarterDeck> decks)
		{
			if (ScreenManagement.ScreenState != CardTemple.Undead)
				decks.RemoveAll(x => x.Info.title.Contains("GrimoraDeck_"));
			else
				decks.RemoveAll(x => x.Info.title.Contains("StockDeck_"));
			return decks;
		};
	}
	private void CreatePack()
	{
		PackInfo cardPack = PackManager.GetPackInfo<PackInfo>(PluginGUID);
		cardPack.Title = "Revenant Mod Card Pack";
		cardPack.Description = "This card pack consists of mainly cards that return to the player in a variety of ways.";
		cardPack.SetTexture(Loader.LoadTexture("pack_revenant"));
		cardPack.ValidFor.Add(PackInfo.PackMetacategory.LeshyPack);
		cardPack.ValidFor.Add(PackInfo.PackMetacategory.GrimoraPack);
		cardPack.ValidFor.Add(PackInfo.PackMetacategory.P03Pack);
		cardPack.SplitPackByCardTemple = false;
	}
}
