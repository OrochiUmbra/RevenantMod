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
        return TextureHelper.GetImageAsTexture(((pixel ? "pixel" : "") + "ability_" + name + (name.EndsWith(".png") ? "" : ".png")).ToLowerInvariant(), CurrentAssembly);
    }
    public static Texture2D LoadPortrait(string name, bool pixel = false)
    {
        return TextureHelper.GetImageAsTexture(((pixel ? "pixel" : "") + "portrait_" + name + (name.EndsWith(".png") ? "" : ".png")).ToLowerInvariant(), CurrentAssembly);
    }
    public static Texture2D LoadEmission(string name)
    {
        return TextureHelper.GetImageAsTexture(("portrait_" + name + (name.EndsWith("_emission.png") ? "" : "_emission.png")).ToLowerInvariant(), CurrentAssembly);
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
