using DiskCardGame;
using InscryptionAPI.Card;

namespace RevenantMod;

public partial class RevenantMod
{
    private void Add_Card_Limbs()
    {
        const string cardName = "Limbs";
        const string cardDescription = "Once attached to a living person, they now belong to something else entirely.";
        const CardTemple Part2CardTemple = CardTemple.Undead;

        CardManager.New(modPrefix: PluginGUID, cardName, "Possessed Limbs", 0, 2, cardDescription)
        .SetRare()
        .SetCardTemple(Part2CardTemple)
        .SetCost(bonesCost: 4)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
        .SetEmissivePortrait(Loader.LoadEmission(cardName))
        .AddAbilities(Ability.Sentry, Ability.Evolve)
        .AddTribes(Tribe.Canine)
        .SetEvolve($"{PluginGUID}_Reanimated", 2)
        .AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, NeutralRegion)
        ;
    }
}
