using DiskCardGame;
using InscryptionAPI.Card;

namespace RevenantMod;

public partial class RevenantMod
{
    private void Add_Card_WhistleWind()
    {
        const string cardName = "WhistleWind";
        const string cardDescription = "A card that wins. It always wins. That's why it's called Mister Win.";
        const CardTemple Part2CardTemple = CardTemple.Undead;

        CardManager.New(modPrefix: PluginGUID, cardName, "Mister Win", 9, 9, cardDescription)
        .SetRare()
        .SetCardTemple(Part2CardTemple)
        .SetOnePerDeck()
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))//.SetPixelPortrait(ResourceBank.Get<Texture2D>("Art/GBC/Cards/PixelPortraits/pixelportrait_headlesshorseman"))
        .SetPortrait(Loader.LoadPortrait(cardName))
        .SetEmissivePortrait(Loader.LoadEmission(cardName))
        .AddAbilities(MacabreBonePulse.ability)
        .AddTribes(Tribe.Hooved)
        .AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, NeutralRegion)
        ;
    }
}
