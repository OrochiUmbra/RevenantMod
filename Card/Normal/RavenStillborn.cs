using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;

namespace RevenantMod;

public partial class RevenantMod
{
    private void Add_Card_RavenStillborn()
    {
        const string cardName = "RavenStillborn";
        const string cardDescription = "Feels good to be young again!";
        const CardTemple Part2CardTemple = CardTemple.Nature;

        CardManager.New(modPrefix: PluginGUID, cardName, "Stillborn Raven", 1, 1, cardDescription)
        .SetDefaultPart1Card()
        .SetCardTemple(Part2CardTemple)
        .SetCost(bonesCost: 2)
        .SetPixelPortrait(ResourceBank.Get<Texture2D>("Art/GBC/Cards/PixelPortraits/pixelportrait_hydraegg"))
        .SetPortrait(Loader.LoadPortrait(cardName))
        .SetEmissivePortrait(Loader.LoadEmission(cardName))
        .SetEvolve("RavenEgg", 1)
        .AddAbilities(LatchEvolve.ability, Ability.Brittle)
        .AddTribes(Tribe.Bird, Tribe.Insect)
        .AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, NeutralRegion)
        ;
    }
}
