using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;

namespace RevenantMod;

public partial class RevenantMod
{
    private void Add_Card_Exorcist()
    {
        const string cardName = "Exorcist";
        const string cardDescription = "He still thinks banishing the dammed will do him good, does he know where he is?";
        const CardTemple Part2CardTemple = CardTemple.Undead;

        CardManager.New(modPrefix: PluginGUID, cardName, cardName.SplitPascalCase(), 1, 2, cardDescription)
        .SetRare()
        .SetCardTemple(Part2CardTemple)
        .SetCost(energyCost: 6)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
        .SetEmissivePortrait(Loader.LoadEmission(cardName))
        .AddAbilities(OpposingBonesCost.ability, Ability.Tutor)
        .AddTribes(Tribe.Reptile)
        .AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, NeutralRegion)
        ;
    }
}

