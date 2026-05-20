
using System.Linq;
using HarmonyLib;
using UnityEngine;

public static class WeaponOutlineColors
{
    private static Material[] _weaponMaterials;

    private static readonly int _weaponOutline = Shader.PropertyToID("_Color_Outline");
    private static readonly int _textOutline = Shader.PropertyToID("_OutlineColor");

    internal static void GetWeaponMaterials()
    {
        var outlineShaderID = Shader.Find("S_WeaponOutline_00").GetInstanceID();
        _weaponMaterials =
            Resources.FindObjectsOfTypeAll<Material>()
            .Where(mat => mat.shader.GetInstanceID() == outlineShaderID).ToArray();
    }

    internal static void SetColor(Color weaponColor, Color textColor)
    {
        Debug.LogError(PauseManager.Instance.grabPopup.fontMaterial.GetColor("_OutlineColor").ToString());
        _weaponMaterials.Do(mat => mat.SetColor(_weaponOutline, weaponColor));

        // Yoinked from kestrel
        var HDR_color = textColor * new Vector4(2, 2, 2, 1);
        PauseManager.Instance.grabPopup.fontMaterial.SetColor(_textOutline, HDR_color);
        PauseManager.Instance.interactPopup.fontMaterial.SetColor(_textOutline, HDR_color);
    }
}
