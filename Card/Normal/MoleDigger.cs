using DiskCardGame;
using InscryptionAPI.Card;

namespace RevenantMod;

public partial class RevenantMod
{
    private void Add_Card_MoleDigger()
    {
        const string cardName = "MoleDigger";
        const string cardDescription = "The stalwart Mole... Digger? Whatever this is...";
        const CardTemple Part2CardTemple = CardTemple.Nature;

        CardManager.New(modPrefix: PluginGUID, cardName, "Mole Digger", 0, 4, cardDescription)
        .SetDefaultPart1Card()
        .SetCardTemple(Part2CardTemple)
        .SetCost(energyCost: 3)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
        .SetEmissivePortrait(Loader.LoadEmission(cardName))
        .AddAbilities(Ability.WhackAMole, Ability.BoneDigger)
        .AddTribes(Tribe.Reptile)
        .AddTraits(Trait.Blind)
        .AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, UndeadRegion, NatureRegion)
        ;
    }
}
