using System.Collections.Generic;
using System.Linq;
using BepInEx;
using ComputerysModdingUtilities;
using HarmonyLib;
using ModMenu.Api;
using UnityEngine;

[assembly: StraftatMod(isVanillaCompatible: true)]

[BepInDependency(ModMenu.PluginInfo.guid, BepInDependency.DependencyFlags.SoftDependency)]
[BepInPlugin("com.koki.tweaks", "STRAFTAT Tweaks", "1.0.0")]
public class STRAFTweakPlugin : BaseUnityPlugin
{
    internal static STRAFTweakPlugin Instance;

    private void Awake()
    {
        Instance = this;
        this.gameObject.hideFlags = HideFlags.HideAndDontSave;

        // Weapon outline color
        WeaponOutlineColors.UpdateColorsFromConfig();
        if (WeaponOutlineColors.ModMenuCompat.enabled)
            ModMenuCustomisation.RegisterContentBuilder(WeaponOutlineColors.ModMenuCompat.WeaponColorBuilder);
    }
}