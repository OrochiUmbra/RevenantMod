using DiskCardGame;
using InscryptionAPI.Card;
using System.Linq;
using UnityEngine;

namespace RevenantMod;

public class BoneDiggerAttack : VariableStatBehaviour
{
    public static SpecialStatIcon icon;
    public static SpecialStatIcon Icon => icon;
    public override SpecialStatIcon IconType => icon;

    // { Power , health }
    public override int[] GetStatValues()
    {
        int digBuff = 0;
        foreach (CardSlot slot in Singleton<BoardManager>.Instance.AllSlotsCopy.Where(slot => slot.Card != null && !slot.Card.Dead))
            digBuff += slot.Card.AllAbilities().FindAll(a => AbilitiesUtil.GetInfo(a).rulebookDescription.ToLowerInvariant().Contains("bone") || AbilitiesUtil.GetInfo(a).rulebookDescription.ToLowerInvariant().Contains("dig")).Count;
        return new int[2] { digBuff, 0 };
    }
}

public partial class RevenantMod
{
    private void Add_Ability_Icon_BoneDiggerAttack()
    {
        const string rulebookName = nameof(BoneDiggerAttack);
        const string rulebookDescription = "The value represented by this sigil will be equal to the number of Bone and Digger related sigils on the board.";

        StatIconInfo statIconInfo = StatIconManager.New(PluginGUID, "Bone Digger Power", rulebookDescription, typeof(BoneDiggerAttack))
        .SetDefaultPart1Ability()
        .SetIcon(ResourceBank.Get<Texture2D>("Art/Cards/AbilityIcons/ability_bonedigger"))
        .SetPixelIcon(Loader.LoadTexture("pixelstaticon_" + rulebookName), FilterMode.Point);//.SetPixelIcon(ResourceBank.Get<Texture2D>("Art/GBC/Cards/PixelAbilityIcons/pixelability_bonedigger"));
        statIconInfo.appliesToAttack = true;
        statIconInfo.appliesToHealth = false;

        BoneDiggerAttack.icon = StatIconManager.Add(PluginGUID, statIconInfo, typeof(BoneDiggerAttack)).Id;
    }
}
