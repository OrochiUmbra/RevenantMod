using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;

namespace RevenantMod;

public partial class RevenantMod
{
    private void Add_Card_Reanimated()
    {
        const string cardName = "Reanimated";
        //const string cardDescription = "";
        const CardTemple Part2CardTemple = CardTemple.Undead;

        CardManager.New(modPrefix: PluginGUID, cardName, cardName.SplitPascalCase(), 1, 4, null)
        .AddAppearances(CardAppearanceBehaviour.Appearance.RareCardBackground)
        .SetCardTemple(Part2CardTemple)
        .SetCost(bonesCost: 4)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
        .SetEmissivePortrait(Loader.LoadEmission(cardName))
        .AddAbilities(Ability.Sentry, Ability.Sentry, Ability.GuardDog)
        .AddTribes(Tribe.Canine)
        ;
    }
}
