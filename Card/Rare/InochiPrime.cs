using DiskCardGame;
using InscryptionAPI.Card;

namespace RevenantMod;

public partial class RevenantMod
{
    private void Add_Card_InochiPrime()
    {
        const string cardName = "InochiPrime";
        const string cardDescription = "A soothing presence to heal your creatures, a calm before the storm.";
        const CardTemple Part2CardTemple = CardTemple.Nature;

        CardManager.New(modPrefix: PluginGUID, cardName, "Inochi", 1, 2, cardDescription)
        .SetRare()
        .SetCardTemple(Part2CardTemple)
        .SetOnePerDeck()
        .SetCost(energyCost: 3)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
        .SetEmissivePortrait(Loader.LoadEmission(cardName))
        .SetExtendedProperty("TransmuteForm", $"{PluginGUID}_InochiUmbra")
        .AddAbilities(ActivatedTransform.ability, SoothingMeadows.ability)
        .AddTribes(Tribe.Hooved, Tribe.Reptile)   
        .AddSpecialAbilities(InochiCalamity.specialAbility)
        .AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, NeutralRegion)
        ;
    }
}
