using DiskCardGame;
using InscryptionAPI.Card;

namespace RevenantMod;

public partial class RevenantMod
{
    private void Add_Card_AnneBean()
    {
        const string cardName = "AnneBean";
        const string cardDescription = "That ink keeps on pouring, is she even there anymore? All that death promises is that you will wake up again.";
        const CardTemple Part2CardTemple = CardTemple.Undead;

        CardManager.New(modPrefix: PluginGUID, cardName, "Anneta", 1, 2, cardDescription)
        .SetRare()
        .SetCardTemple(Part2CardTemple)
        .SetOnePerDeck()
        .SetCost(bonesCost: 3)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
        .SetEmissivePortrait(Loader.LoadEmission(cardName))
        .AddAbilities(PartingStroke.ability)
        .AddTribes(Tribe.Reptile)
        .AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, NeutralRegion)
        ;
    }
}
