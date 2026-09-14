using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;

namespace RevenantMod;

public partial class RevenantMod
{
    private void Add_Card_FulmineSkull()
    {
        const string cardName = "FulmineSkull";
        const string cardDescription = "If you listen closely, you can still hear the cackling of flame on bone... as well as their voice.";
        const CardTemple Part2CardTemple = CardTemple.Undead;

        CardManager.New(modPrefix: PluginGUID, cardName, cardName.SplitPascalCase(), 1, 1, cardDescription)
        .SetRare()
        .SetCardTemple(Part2CardTemple)
        .SetCost(bonesCost: 2)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
        .SetEmissivePortrait(Loader.LoadEmission(cardName))
        .AddAbilities(DrawCopySkeleton.ability, Ability.ExplodeOnDeath)
        .AddTribes(Tribe.Reptile)
        .AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, NeutralRegion)
        ;
    }
}
