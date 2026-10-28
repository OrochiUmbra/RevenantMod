using DiskCardGame;
using InscryptionAPI.Helpers;
using System.Reflection;
using UnityEngine;

namespace RevenantMod;

internal class Loader
{
    private static Assembly _assembly;
    public static Assembly CurrentAssembly => _assembly ??= Assembly.GetExecutingAssembly();

    public static Texture2D LoadIcon(string name, bool pixel = false)
    {
        string path = (pixel ? "pixel" : "") + "ability_" + name + (name.EndsWith(".png") ? "" : ".png");
        return TryLoad(path, () => LoadPHIcon(pixel));
    }
    public static Texture2D LoadPortrait(string name, bool pixel = false)
    {
        string path = (pixel ? "pixel" : "") + "portrait_" + name + (name.EndsWith(".png") ? "" : ".png");
        return TryLoad(path, () => LoadPHPortrait(pixel));
    }
    public static Texture2D LoadEmission(string name)
    {
        string path = "portrait_" + name + (name.EndsWith("_emission.png") ? "" : "_emission.png");
        return TryLoad(path, () => LoadPHEmission());
    }
    public static Texture2D LoadPHIcon(bool pixel = false)
    {
        return pixel ? ResourceBank.Get<Texture2D>("Art/GBC/Cards/PixelAbilityIcons/pixelability_hydraegg") : (Texture2D)AbilitiesUtil.LoadAbilityIcon(Ability.RandomAbility.ToString());
    }
    public static Texture2D LoadPHPortrait(bool pixel = false)
    {
        return pixel ? ResourceBank.Get<Texture2D>("Art/GBC/Cards/PixelPortraits/pixelportrait_cat") : ResourceBank.Get<Texture2D>("Art/Cards/Portraits/portrait_cat");
    }
    public static Texture2D LoadPHEmission()
    {
        return ResourceBank.Get<Texture2D>("Art/Cards/Portraits/portrait_cat_emission");
    }
    private static Texture2D TryLoad(string path, System.Func<Texture2D> fallback)
    {
        try
        {
            Texture2D tex = TextureHelper.GetImageAsTexture(path.ToLowerInvariant(), CurrentAssembly);
            return tex ?? fallback();
        }
        catch
        {
            return fallback();
        }
    }

    public static Texture2D LoadTexture(string name)
    {
        return TextureHelper.GetImageAsTexture((name + (name.EndsWith(".png") ? "" : ".png")).ToLowerInvariant(), CurrentAssembly);
    }
    public static Sprite LoadSprite(string name)
    {
        return TextureHelper.GetImageAsTexture((name + (name.EndsWith(".png") ? "" : ".png")).ToLowerInvariant(), CurrentAssembly).ConvertTexture();
    }
}
