using System.Collections.Generic;
using System.Linq;
using BepInEx;
using ComputerysModdingUtilities;
using HarmonyLib;
using UnityEngine;

[assembly: StraftatMod(isVanillaCompatible: true)]

[BepInPlugin("com.koki.tweaks", "STRAFTAT Tweaks", "1.0.0")]
public class STRAFTweakPlugin : BaseUnityPlugin
{
    internal static STRAFTweakPlugin Instance;
    private Material[] _weaponMaterials;

    private void Awake()
    {
        Instance = this;
        this.gameObject.hideFlags = HideFlags.HideAndDontSave;

        // Weapon outline color
        WeaponOutlineColors.InitWeaponMaterials();
        WeaponOutlineColors.UpdateColorsFromConfig();
    }
}