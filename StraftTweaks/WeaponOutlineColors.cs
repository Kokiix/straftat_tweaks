
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
    static Color _weaponColor;
    static Color _weaponTextColor;
    static float _weaponTextBrightness;
    internal static void SetConfigBinds()
    {
        _weaponColor = STRAFTweakPlugin.Instance.Config.Bind("Outline Colors", "Weapon Outline Color", new Color(1, 0.8196079f, 0.427451f)).Value;
        _weaponTextColor = STRAFTweakPlugin.Instance.Config.Bind("Outline Colors", "Weapon Interact Text Color", new Color(2, 1.106f, 0)).Value;
        _weaponTextBrightness = STRAFTweakPlugin.Instance.Config.Bind("Outline Colors", "Weapon Interact Text Brightness", 1f).Value;
    }

    static bool HasRanOnStartup = false;
    static void Postfix()
    {
        if (!HasRanOnStartup)
        {
            UpdateColorsFromConfig();
            HasRanOnStartup = true;
        }
    }

    static readonly int _weaponOutline = Shader.PropertyToID("_Color_Outline");
    static readonly int _textOutline = Shader.PropertyToID("_OutlineColor");
    static readonly int outlineShaderID = Shader.Find("S_WeaponOutline_00").GetInstanceID();
    static Lazy<Material[]> _weaponMaterials = new(() =>
    {
        return Resources.FindObjectsOfTypeAll<Material>()
        .Where(mat => mat.shader.GetInstanceID() == outlineShaderID).ToArray();
    });

    internal static void UpdateColorsFromConfig()
    {
        SetConfigBinds();

        // Update stored materials
        _weaponMaterials.Value.Do(mat => mat.SetColor(_weaponOutline, _weaponColor));

        // Update live materials
        foreach (var renderer in UnityEngine.Object.FindObjectsOfType<Renderer>())
        {
            foreach (var mat in renderer.sharedMaterials)
            {
                if (mat && mat.shader.GetInstanceID() == outlineShaderID)
                    mat.SetColor(_weaponOutline, _weaponColor);
            }
        }

        // Weapon text yoinked from kestrel; Default text is RGBA(2.000, 1.106, 0.000, 1.000)
        var HDR_color = _weaponTextColor * Vector4.one * _weaponTextBrightness;
        PauseManager.Instance.grabPopup.fontSharedMaterial.SetColor(_textOutline, HDR_color);
        PauseManager.Instance.interactPopup.fontSharedMaterial.SetColor(_textOutline, HDR_color);
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