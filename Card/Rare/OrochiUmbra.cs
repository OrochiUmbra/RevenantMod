using DiskCardGame;
using InscryptionAPI.Card;

namespace RevenantMod;

public partial class RevenantMod
{
    private void Add_Card_OrochiUmbra()
    {
        const string cardName = "OrochiUmbra";
        const string cardDescription = "From within the endless torment and suffering, they had somehow learned a way to see inside this poor broken thing... and take away its pain.";
        const CardTemple Part2CardTemple = CardTemple.Undead;

        CardManager.New(modPrefix: PluginGUID, cardName, "Master Ouro", 1, 1, cardDescription)
        .SetRare()
        .SetCardTemple(Part2CardTemple)
        .SetOnePerDeck()
        .SetCost(energyCost: 2)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
        .SetEmissivePortrait(Loader.LoadEmission(cardName))
        .AddAbilities(Limbo.ability, Necrosis.ability)
        .AddTribes(Tribe.Hooved, Tribe.Reptile)
        .AddSpecialAbilities(OuroCoreEnergy.specialAbility)
        .AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, NeutralRegion)
        ;
    }
}
