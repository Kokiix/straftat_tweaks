
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using HarmonyLib;
using ModMenu.Api;
using UnityEngine;

namespace WeaponOutlineColors;

[HarmonyPatch(typeof(PauseManager), "Awake")]
static class SetOutlineColors
{
    // Hmmm this Resources.FindObjectsOfTypeAll call isn't even compatible with my own weapons mod... I think that's my other mod's fault tho
    static Lazy<Material[]> _weaponMaterials = new(() =>
    {
        var outlineShaderID = Shader.Find("S_WeaponOutline_00").GetInstanceID();
        return Resources.FindObjectsOfTypeAll<Material>()
        .Where(mat => mat.shader.GetInstanceID() == outlineShaderID).ToArray();
    });


    static readonly int _weaponOutline = Shader.PropertyToID("_Color_Outline");
    static readonly int _textOutline = Shader.PropertyToID("_OutlineColor");

    internal static void UpdateColorsFromConfig()
    {
        var weaponColor = STRAFTweakPlugin.Instance.Config.Bind("General", "Weapon Outline Color", new Color(255, 209, 109)).Value;
        var weaponTextColor = STRAFTweakPlugin.Instance.Config.Bind("General", "Weapon Interact Text Color", new Color(255, 141, 0)).Value;
        var weaponTextBrightness = STRAFTweakPlugin.Instance.Config.Bind("General", "Weapon Interact Text Brightness", 2).Value;

        // Weapon mat
        _weaponMaterials.Value.Do(mat => mat.SetColor(_weaponOutline, weaponColor));

        // Weapon text yoinked from kestrel
        var HDR_color = weaponTextColor * new Vector4(weaponTextBrightness, weaponTextBrightness, weaponTextBrightness, 1);
        PauseManager.Instance.grabPopup.fontMaterial.SetColor(_textOutline, HDR_color);
        PauseManager.Instance.interactPopup.fontMaterial.SetColor(_textOutline, HDR_color);
    }

    static void Postfix()
    {
        Debug.LogError("test");
        UpdateColorsFromConfig();
    }
}

public class ModMenuCompat
{
    internal bool Enabled { get => BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(ModMenu.PluginInfo.guid); }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public void ConfigBuilder(OptionListContext c)
    {
        c.InsertHeader(13, "Test Section 1");
    }
}