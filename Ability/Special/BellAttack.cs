using DiskCardGame;
using InscryptionAPI.Card;
using System.Linq;
using UnityEngine;

namespace RevenantMod;

public class BellAttack : VariableStatBehaviour
{
    public static SpecialStatIcon icon;
    public static SpecialStatIcon Icon => icon;
    public override SpecialStatIcon IconType => icon;

    // { Power , health }
    public override int[] GetStatValues()
    {
        int bellBuff = 0;
        foreach (CardSlot slot in Singleton<BoardManager>.Instance.AllSlotsCopy.Where(slot => slot.Card != null && !slot.Card.Dead && slot.Card.Info.name.ToLowerInvariant().Contains("bell")))
            bellBuff++;
        return new int[2] { bellBuff, 0 };
    }
}

public partial class RevenantMod
{
    private void Add_Ability_Icon_BellAttack()
    {
        const string rulebookName = nameof(BellAttack);
        const string rulebookDescription = "The value represented by this sigil will be equal to the number of Bell cards on the board.";

        StatIconInfo statIconInfo = StatIconManager.New(PluginGUID, "Chime Power", rulebookDescription, typeof(BellAttack))
        .SetDefaultPart1Ability()
        .SetIcon(ResourceBank.Get<Texture2D>("Art/Cards/AbilityIcons/ability_createbells"))
        .SetPixelIcon(Loader.LoadTexture("pixelstaticon_" + rulebookName), FilterMode.Point);//.SetPixelIcon(ResourceBank.Get<Texture2D>("Art/GBC/Cards/PixelAbilityIcons/pixelability_conduitbuffattack"));
        statIconInfo.appliesToAttack = true;
        statIconInfo.appliesToHealth = false;

        BellAttack.icon = StatIconManager.Add(PluginGUID, statIconInfo, typeof(BellAttack)).Id;
    }
}
