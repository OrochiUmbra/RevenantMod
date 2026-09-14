using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;
using UnityEngine;

namespace RevenantMod;

public partial class RevenantMod
{
    private void Add_Card_Orochi()
    {
        const string cardName = "Orochi";
        const string cardDescription = "Sacrifices must be made.";
        const CardTemple Part2CardTemple = CardTemple.Undead;

        CardManager.New(modPrefix: PluginGUID, cardName, cardName.SplitPascalCase(), 1, 8, cardDescription)
        .SetCardTemple(Part2CardTemple)
        .SetTerrain(false)
        .SetOnePerDeck()
        .SetCost(bonesCost: 8, energyCost: 6)
        .SetPixelPortrait(ResourceBank.Get<Texture2D>("Art/GBC/Cards/PixelPortraits/pixelportrait_ouroboros"))
        .SetPortrait(Loader.LoadPortrait(cardName))
        .SetEmissivePortrait(Loader.LoadEmission(cardName))
        .AddAppearances(CardAppearanceBehaviour.Appearance.RareCardBackground)
        .AddTraits(Trait.Uncuttable, Trait.KillsSurvivors, Trait.Fused, Trait.DeathcardCreationNonOption)
        .AddAbilities(Ability.TriStrike, Ability.GainAttackOnKill, Ability.SplitStrike, Ability.MadeOfStone)
        .AddTribes(Tribe.Reptile, Tribe.Canine, Tribe.Hooved, Tribe.Insect)
        ;
    }
}
