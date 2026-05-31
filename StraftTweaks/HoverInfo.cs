
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace HoverInfo;

[HarmonyPatch(typeof(PauseManager), "Awake")]
static class OutlineColor
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

    internal static void UpdateColorsFromConfig(object sender, EventArgs args)
    {
        UpdateColorsFromConfig();
    }

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
    internal static ConfigEntry<bool> enablePrompt;
    internal static ConfigEntry<bool> disableKey;

    // Could be transpiled to avoid double string set / extra function call
    static void Postfix(ItemBehaviour __instance)
    {
        PauseManager.Instance.grabPopup.gameObject.SetActive(enablePrompt.Value);
        if (disableKey.Value)
            PauseManager.Instance.grabPopup.text = __instance.weaponName.ToLower();
    }
}

[HarmonyPatch(typeof(Door), "OnFocus")]
static class DoorInteractPrompt
{
    internal static ConfigEntry<bool> enablePrompt;

    static void Postfix(Door __instance)
    {
        PauseManager.Instance.interactPopup.gameObject.SetActive(enablePrompt.Value);
        if (WeaponInteractPrompt.disableKey.Value)
            PauseManager.Instance.interactPopup.text = (__instance.sync___get_value_isOpen() ? __instance.closeDoor.ToLower() : __instance.popupText.ToLower());
    }
}

class RainbowOutline : MonoBehaviour
{
    void Update()
    {
        Debug.LogError("test");
    }
}