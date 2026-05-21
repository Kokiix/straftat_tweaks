using BepInEx;
using ComputerysModdingUtilities;
using HarmonyLib;
using ModMenu.Api;
using UnityEngine;
using WeaponOutlineColors;

[assembly: StraftatMod(isVanillaCompatible: true)]

[BepInDependency(ModMenu.PluginInfo.guid, BepInDependency.DependencyFlags.SoftDependency)]
[BepInPlugin("com.koki.tweaks", "STRAFTAT Tweaks", "1.0.0")]
public class STRAFTweakPlugin : BaseUnityPlugin
{
    internal static STRAFTweakPlugin Instance;

    Harmony _harmony = new("com.koki.tweaks");

    void Awake()
    {
        _harmony.PatchAll();
        Instance = this;
        this.gameObject.hideFlags = HideFlags.HideAndDontSave;

        // Weapon outline color
        var compat = new WeaponOutlineColors.ModMenuCompat();
        if (compat.Enabled)
        {
            Debug.LogError("running!");
            ModMenuCustomisation.RegisterContentBuilder(compat.ConfigBuilder);
        }
    }

    void OnDestroy()
    {
        _harmony.UnpatchSelf();
    }
}