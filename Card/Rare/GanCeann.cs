using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;

namespace RevenantMod;

public partial class RevenantMod
{
    private void Add_Card_P03()
    {
        const string cardName = "GanCeann";
        const string cardDescription = "Countless legends tell of the dullahan, but all fail to convey the true extent of its pique.";
        const CardTemple Part2CardTemple = CardTemple.Undead;

        CardManager.New(modPrefix: PluginGUID, cardName, cardName.SplitPascalCase(), 1, 1, cardDescription)
        .SetRare()
        .SetCardTemple(Part2CardTemple)
        .SetCost(energyCost: 3)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
        .SetEmissivePortrait(Loader.LoadEmission(cardName))
        .AddAbilities(SlotBuffPower.ability)
        .AddTribes(Tribe.Bird)
        .AddMetaCategories(GrimoraChoice, NeutralRegion)
        ;
    }
}
