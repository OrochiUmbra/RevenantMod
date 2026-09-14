using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;

namespace RevenantMod;

public partial class RevenantMod
{
    private void Add_Card_SpadeWarrior()
    {
        const string cardName = "SpadeWarrior";
        const string cardDescription = "This fearsome warrior will put you six feet under, in every possible sense.";
        const CardTemple Part2CardTemple = CardTemple.Undead;

        CardManager.New(modPrefix: PluginGUID, cardName, cardName.SplitPascalCase(), 0, 3, cardDescription)
        .SetRare()
        .SetCardTemple(Part2CardTemple)
        .SetCost(bonesCost: 5)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
        .SetEmissivePortrait(Loader.LoadEmission(cardName))
        .AddAbilities(LatchDiggerPrime.ability)
        .AddTribes(Tribe.Reptile)
        .SetStatIcon(BoneDiggerAttack.icon)
        .AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, NeutralRegion)
        ;
    }
}
