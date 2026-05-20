
using System.Linq;
using HarmonyLib;
using UnityEngine;

public static class WeaponOutlineColors
{
    private static Material[] _weaponMaterials;

    private static readonly int _outlineProperty = Shader.PropertyToID("_Color_Outline");

    internal static void Init()
    {
        GetWeaponMaterials();
    }

    private static void GetWeaponMaterials()
    {
        var outlineShaderID = Shader.Find("S_WeaponOutline_00").GetInstanceID();
        _weaponMaterials =
            Resources.FindObjectsOfTypeAll<Material>()
            .Where(mat => mat.shader.GetInstanceID() == outlineShaderID).ToArray();
    }

    internal static void SetColor(Color c)
    {
        _weaponMaterials.Do(mat => mat.SetColor(_outlineProperty, c));

        // Yoinked from kestrel
        PauseManager.Instance.grabPopup.fontMaterial.SetColor(m_propTextOutlineColor, color);
        PauseManager.Instance.interactPopup.fontMaterial.SetColor(m_propTextOutlineColor, color);
    }
}
