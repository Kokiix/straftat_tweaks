
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace WeaponOutlines;

static class Config
{
    internal static void SetBinds()
    {

        SetColors.weaponColor = STRAFTweakPlugin.Instance.Config.Bind("Outline Colors", "Weapon Outline Color", new Color(1, 0.8196079f, 0.427451f));
        SetColors.weaponTextColor = STRAFTweakPlugin.Instance.Config.Bind("Outline Colors", "Weapon Interact Text Color", new Color(2, 1.106f, 0));
        SetColors.weaponTextBrightness = STRAFTweakPlugin.Instance.Config.Bind("Outline Colors", "Weapon Interact Text Brightness", 1f);

        WeaponInteractPrompt.enable = STRAFTweakPlugin.Instance.Config.Bind("Hover Prompts", "Enable Weapon Interact Text", true);
        WeaponInteractPrompt.keyPrompt = STRAFTweakPlugin.Instance.Config.Bind("Hover Prompts", "Enable Weapon Interact Key Prompt", false, "Transform \"Handgun [F]\" into just \"Handgun\".");
    }
}

[HarmonyPatch(typeof(PauseManager), "Awake")]
static class SetColors
{
    internal static ConfigEntry<Color> weaponColor;
    internal static ConfigEntry<Color> weaponTextColor;
    internal static ConfigEntry<float> weaponTextBrightness;

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
        // Update stored materials
        _weaponMaterials.Value.Do(mat => mat.SetColor(_weaponOutline, weaponColor.Value));

        // Update live materials
        foreach (var renderer in UnityEngine.Object.FindObjectsOfType<Renderer>())
        {
            foreach (var mat in renderer.sharedMaterials)
            {
                if (mat && mat.shader.GetInstanceID() == outlineShaderID)
                    mat.SetColor(_weaponOutline, weaponColor.Value);
            }
        }

        // Weapon text yoinked from kestrel; Default text is RGBA(2.000, 1.106, 0.000, 1.000)
        var HDR_color = weaponTextColor.Value * Vector4.one * weaponTextBrightness.Value;
        PauseManager.Instance.grabPopup.fontSharedMaterial.SetColor(_textOutline, HDR_color);
        PauseManager.Instance.interactPopup.fontSharedMaterial.SetColor(_textOutline, HDR_color);
    }
}

[HarmonyPatch(typeof(ItemBehaviour), "OnFocus")]
static class WeaponInteractPrompt
{
    internal static ConfigEntry<bool> enable;
    internal static ConfigEntry<bool> keyPrompt;

    // Could be transpiled to avoid double string set / extra function call
    static void Postfix(ItemBehaviour __instance)
    {
        PauseManager.Instance.grabPopup.gameObject.SetActive(enable.Value);
        if (!keyPrompt.Value)
            PauseManager.Instance.grabPopup.text = __instance.weaponName.ToLower();
    }
}