using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;

namespace RevenantMod;

public partial class RevenantMod
{
    private void Add_Card_Bloated()
    {
        const string cardName = "TheBloated";
        const string cardDescription = "Grotesque boils, ready to burst.";
        const CardTemple Part2CardTemple = CardTemple.Undead;

        CardManager.New(modPrefix: PluginGUID, cardName, cardName.SplitPascalCase(), 1, 1, cardDescription)
        .SetDefaultPart1Card()
        .SetCardTemple(Part2CardTemple)
        .SetCost(bonesCost: 3)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
        .SetEmissivePortrait(Loader.LoadEmission(cardName))
        .AddAbilities(ExplodingCorpse.ability)
        .AddTribes(Tribe.Canine)
        .AddTraits(Trait.KillsSurvivors)
        .AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, UndeadRegion, NatureRegion)
        ;
    }
}
