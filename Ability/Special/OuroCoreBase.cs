using DiskCardGame;
using UnityEngine;

namespace RevenantMod;

public class OuroCoreBase : SpecialCardBehaviour, IModifyDeadCard
{
    public bool RespondsToModifyDeadCard(CardInfo deadCard, bool wasSacrifice, bool opponentCard, CardInfo killer)
    {
        return true;
    }

    public CardInfo OnModifyDeadCard(CardInfo deadCard, bool wasSacrifice, bool opponentCard, CardInfo killer)
    {
        Debug.Log($"OuroCoreBase running");
        deadCard.Mods.Add(new(1, 1));
        return ModifyOuroCard(deadCard);
    }

    // override with modifications
    public virtual CardInfo ModifyOuroCard(CardInfo ouroCard)
    {
        return ouroCard;
    }
}
