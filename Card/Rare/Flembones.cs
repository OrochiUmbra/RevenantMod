using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;

namespace RevenantMod;

public partial class RevenantMod
{
    private void Add_Card_Flembones()
    {
        const string cardName = "Flembones";
        const string cardDescription = "Wait until he plays with his webcam.";
        const CardTemple Part2CardTemple = CardTemple.Undead;

        CardManager.New(modPrefix: PluginGUID, cardName, cardName.SplitPascalCase(), 0, 2, cardDescription)
        .SetRare()
        .SetCardTemple(Part2CardTemple)
        .SetCost(bonesCost: 5)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
        .SetEmissivePortrait(Loader.LoadEmission(cardName))
        .AddAbilities(DeathMetal.ability)
        .AddTribes(Tribe.Hooved)
        .AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, NeutralRegion)
        ;
    }
}
