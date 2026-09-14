using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;

namespace RevenantMod;

public partial class RevenantMod
{
    private void Add_Card_OdiousMass()
    {
        const string cardName = "OdiousMass";
        const string cardDescription = "How unsightly.";
        const CardTemple Part2CardTemple = CardTemple.Nature;

        CardManager.New(modPrefix: PluginGUID, cardName, cardName.SplitPascalCase(), 1, 1, cardDescription)
        .SetDefaultPart1Card()
        .SetCardTemple(Part2CardTemple)
        .SetCost(bonesCost: 3)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
        .SetEmissivePortrait(Loader.LoadEmission(cardName))
        .AddAbilities(DrawCopyDelay.ability)
        .AddTribes(Tribe.Canine)
        .AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, UndeadRegion, NatureRegion)
        ;
    }
}
