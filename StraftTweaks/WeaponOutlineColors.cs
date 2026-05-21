
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
    // Resources.FindObjectsOfTypeAll won't work my own weapons mod lol (for now)
    static Lazy<Material[]> _weaponMaterials = new(() =>
    {
        var outlineShaderID = Shader.Find("S_WeaponOutline_00").GetInstanceID();
        return Resources.FindObjectsOfTypeAll<Material>()
        .Where(mat => mat.shader.GetInstanceID() == outlineShaderID).ToArray();
    });

    static Color _weaponColor;
    static Color _weaponTextColor;
    static float _weaponTextBrightness;

    internal static void Init()
    {
        _weaponColor = STRAFTweakPlugin.Instance.Config.Bind("Outline Colors", "Weapon Outline Color", new Color(1, 0.8196079f, 0.427451f)).Value;
        _weaponTextColor = STRAFTweakPlugin.Instance.Config.Bind("Outline Colors", "Weapon Interact Text Color", new Color(2, 1.106f, 0)).Value;
        _weaponTextBrightness = STRAFTweakPlugin.Instance.Config.Bind("Outline Colors", "Weapon Interact Text Brightness", 1f).Value;
    }


    static readonly int _weaponOutline = Shader.PropertyToID("_Color_Outline");
    static readonly int _textOutline = Shader.PropertyToID("_OutlineColor");

    internal static void UpdateColorsFromConfig()
    {
        Debug.LogError("trying to change colors");
        // Weapon mat
        _weaponMaterials.Value.Do(mat => mat.SetColor(_weaponOutline, _weaponColor));

        // Weapon text yoinked from kestrel; Default text is RGBA(2.000, 1.106, 0.000, 1.000)
        var HDR_color = _weaponTextColor * Vector4.one * _weaponTextBrightness;
        PauseManager.Instance.grabPopup.fontMaterial.SetColor(_textOutline, HDR_color);
        PauseManager.Instance.interactPopup.fontMaterial.SetColor(_textOutline, HDR_color);
    }

    static void Postfix()
    {
        UpdateColorsFromConfig();
    }
}

static class ModMenu
{
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    internal static void ConfigBuilder(OptionListContext c)
    {
        c.AppendButton("Apply Changes", "Apply Changes", SetOutlineColors.UpdateColorsFromConfig);
    }
}