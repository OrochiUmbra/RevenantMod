using DiskCardGame;
using InscryptionAPI.Card;

namespace RevenantMod;

public partial class RevenantMod
{
    private void Add_Card_InochiUmbra()
    {
        const string cardName = "InochiUmbra";
        const string cardDescription = "Trail of Torment.";

        CardManager.New(modPrefix: PluginGUID, cardName, "Inochi", 1, 2, cardDescription)
        .AddAppearances(CardAppearanceBehaviour.Appearance.RareCardBackground)
        .SetCardTemple(DynamicCardTemple)
        .SetOnePerDeck()
        .SetCost(energyCost: 3)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
        .SetEmissivePortrait(Loader.LoadEmission(cardName))
        .SetExtendedProperty("TransmuteForm", $"{PluginGUID}_InochiPrime")
        .AddAbilities(ActivatedTransform.ability, CalamityEffusion.ability)
        .AddTribes(Tribe.Hooved, Tribe.Reptile)
        .AddSpecialAbilities(InochiCalamity.specialAbility)
        ;
    }
}
