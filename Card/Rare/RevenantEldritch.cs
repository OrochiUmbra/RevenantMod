using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;

namespace RevenantMod;

public partial class RevenantMod
{
    private void Add_Card_RevenantEldritch()
    {
        const string cardName = "RevenantEldritch";
        const string cardDescription = "The source of Calamity, relentless and unerring.";
        const CardTemple Part2CardTemple = CardTemple.Undead;

        CardManager.New(modPrefix: PluginGUID, cardName, cardName.SplitPascalCase(), 1, 6, cardDescription)
        .SetRare()
        .SetCardTemple(Part2CardTemple)
        .SetOnePerDeck()
        .SetCost(bonesCost: 6, energyCost: 6)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
        .SetEmissivePortrait(Loader.LoadEmission(cardName))
        .AddAbilities(RelentlessRevenant.ability, Ability.MadeOfStone)
        .AddTribes(Tribe.Hooved, Tribe.Reptile)
        .AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, NeutralRegion)
        ;
    }
}
