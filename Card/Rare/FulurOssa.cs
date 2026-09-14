using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;

namespace RevenantMod;

public partial class RevenantMod
{
    private void Add_Card_FulurOssa()
    {
        const string cardName = "FulurOssa";
        const string cardDescription = "It never truly left the electric chair.";
        const CardTemple Part2CardTemple = CardTemple.Undead;

        CardManager.New(modPrefix: PluginGUID, cardName, cardName.SplitPascalCase(), 2, 1, cardDescription)
        .SetRare()
        .SetCardTemple(Part2CardTemple)
        .SetCost(bonesCost: 4, energyCost: 3)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
        .SetEmissivePortrait(Loader.LoadEmission(cardName))
        .AddAbilities(LatchGrimoraRandomPrime.ability)
        .AddTribes(Tribe.Reptile)
        .AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, NeutralRegion)
        ;
    }
}
