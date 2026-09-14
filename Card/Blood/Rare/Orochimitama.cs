using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;

namespace RevenantMod;

public partial class RevenantMod
{
    private void Add_Card_Inochi()
    {
        const string cardName = "Orochimitama";
        const string cardDescription = "Oh... He is not supposed to be here...";
        const CardTemple Part2CardTemple = CardTemple.Undead;

        CardInfo info = CardManager.New(modPrefix: PluginGUID, cardName, cardName, 2, 2, cardDescription)
        .SetRare()
        .SetCardTemple(Part2CardTemple)
        .SetCost(bloodCost: 2)
        .SetPortrait(Loader.LoadPortrait(cardName))
        .SetEmissivePortrait(Loader.LoadEmission(cardName))
        .AddTraits(Trait.DeathcardCreationNonOption)
        .AddAbilities(Ability.DrawCopyOnDeath, Ability.GuardDog)
        .AddMetaCategories(GuidManager.GetEnumValue<CardMetaCategory>("arackulele.inscryption.grimoramod", "GrimoraModChoiceNode"));
        info.appearanceBehaviour.Remove(CardAppearanceBehaviour.Appearance.RareCardBackground);
        info.onePerDeck = true;
    }
}