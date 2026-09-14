using DiskCardGame;
using InscryptionAPI.Card;

namespace RevenantMod;

public partial class RevenantMod
{
    private void Add_Blood_Card_ZTestCard()
    {
        CardManager.New(modPrefix: PluginGUID, "ZTestCard", "Test Limbs", 0, 99, "Those parts used to belong to someone, or now some thing...(Test)")
        .AddAppearances(CardAppearanceBehaviour.Appearance.RareCardBackground)
        .SetCost(bloodCost: 1)
        .SetPortrait(Loader.LoadTexture("portrait_limbs.png"))
        .SetEmissivePortrait(Loader.LoadTexture("portrait_limbs_emission.png"))
        .AddAbilities(LatchDiggerPrime.ability, TutorDead.ability, Necrosis.ability)
        .SetStatIcon(BoneDiggerAttack.icon)
        .SetEvolve("Sniper", 1)
        .SetIceCube($"{PluginGUID}_Reanimated")
        ;
    }
}
