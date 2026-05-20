using System.Collections.Generic;
using System.Linq;
using BepInEx;
using ComputerysModdingUtilities;
using HarmonyLib;
using UnityEngine;

[assembly: StraftatMod(isVanillaCompatible: true)]

[BepInPlugin("com.koki.tweaks", "STRAFTAT Tweaks", "1.0.0")]
public class KokiWeaponsPlugin : BaseUnityPlugin
{
    private Material[] _weaponMaterials;

    private void Awake()
    {
        this.gameObject.hideFlags = HideFlags.HideAndDontSave;

        // Weapon outline color
        var weaponColor = Config.Bind("General", "Weapon Outline Color", new Color(255, 209, 109));
        var weaponTextColor = Config.Bind("General", "Weapon Interact Text Color", new Color(255, 141, 0));
        WeaponOutlineColors.GetWeaponMaterials();
        WeaponOutlineColors.SetColor(weaponColor.Value, weaponTextColor.Value);
    }
}