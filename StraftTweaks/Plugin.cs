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
        var color = Config.Bind("General", "Weapon Outline Color", new Color(255, 209, 109));
        WeaponOutlineColors.Init();
        WeaponOutlineColors.SetColor(color.Value);
    }
}