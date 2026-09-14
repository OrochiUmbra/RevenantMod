using DiskCardGame;
using InscryptionAPI.Card;

namespace RevenantMod;

public partial class RevenantMod
{
    private void Add_Card_DysmorphiaRight()
    {
        const string cardName = "DysmorphiaRight";
        const string cardDescription = "Dysmorphia Right.";
        const CardTemple Part2CardTemple = CardTemple.Undead;

        CardManager.New(modPrefix: PluginGUID, cardName, "Dysmorphia", 0, 1, cardDescription)
        .SetCardTemple(Part2CardTemple)
        .SetTerrain()
        .SetCost(bonesCost: 1)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
        .SetEmissivePortrait(Loader.LoadEmission(cardName))
        .AddAbilities(Ability.BuffEnemy)
        .AddTribes(Tribe.Canine)
        ;
    }
}
