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

    internal bool ModMenuCompat { get => BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(ModMenu.PluginInfo.guid); }

    void Awake()
    {
        _harmony.PatchAll();
        Instance = this;
        this.gameObject.hideFlags = HideFlags.HideAndDontSave;

        if (ModMenuCompat)
        {
            ModMenuCustomisation.HideEntry(Config.Bind("Secret :3", "Mod menu only activates if I bind in Awake :(", 0));
            ModMenuCustomisation.RegisterContentBuilder(WeaponOutlineColors.ModMenu.ConfigBuilder);
        }
    }

    void OnDestroy()
    {
        _harmony.UnpatchSelf();
    }
}