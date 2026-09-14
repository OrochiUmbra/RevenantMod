using DiskCardGame;
using InscryptionAPI.Card;

namespace RevenantMod;

public partial class RevenantMod
{
    private void Add_Card_DysmorphiaLeft()
    {
        const string cardName = "DysmorphiaLeft";
        const string cardDescription = "Dysmorphia Left.";
        const CardTemple Part2CardTemple = CardTemple.Undead;

        CardManager.New(modPrefix: PluginGUID, cardName, "Dysmorphia", 0, 1, cardDescription)
        .SetCardTemple(Part2CardTemple)
        .SetTerrain()
        .SetCost(bonesCost: 1)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
        .SetEmissivePortrait(Loader.LoadEmission(cardName))
        .AddAbilities(Ability.DebuffEnemy)
        .AddTribes(Tribe.Canine)
        ;
    }
}
