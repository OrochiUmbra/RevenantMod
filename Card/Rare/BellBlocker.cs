using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;

namespace RevenantMod;

public partial class RevenantMod
{
    private void Add_Card_BellBlocker()
    {
        const string cardName = "BellBlocker";
        const string cardDescription = "It seems like they found a new way to use bells.";
        const CardTemple Part2CardTemple = CardTemple.Undead;

        CardManager.New(modPrefix: PluginGUID, cardName, "Lych Bell", 0, 3, cardDescription)
        .SetRare()
        .SetCardTemple(Part2CardTemple)
        .SetCost(energyCost: 6)
        .SetPixelPortrait(ResourceBank.Get<Texture2D>("Art/GBC/Cards/PixelPortraits/pixelportrait_squidbell"))
        .SetPortrait(ResourceBank.Get<Texture2D>("Art/Cards/Portraits/portrait_dausbell"))
        .SetEmissivePortrait(ResourceBank.Get<Texture2D>("Art/Cards/Portraits/portrait_bull_emission"))
        .AddAbilities(Ability.CreateBells)
        .AddTribes(Tribe.Insect)
        .AddSpecialAbilities(SpecialTriggeredAbility.Daus)
        .SetStatIcon(BellAttack.icon)
        .AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, NeutralRegion)
        ;
    }
}
