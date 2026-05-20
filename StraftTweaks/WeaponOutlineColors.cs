
using System.Linq;
using UnityEngine;

public static class WeaponOutlineColors
{
    private static Material[] _weaponMaterials;

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
}
