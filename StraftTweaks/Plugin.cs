using System;
using BepInEx;
using BepInEx.Configuration;
using ComputerysModdingUtilities;
using HarmonyLib;
using ImprovedRebinds;
using StraftTweaks;
using UnityEngine;
using WeaponOutlineColors;

[assembly: StraftatMod(isVanillaCompatible: true)]

[BepInDependency(ModMenu.PluginInfo.guid, BepInDependency.DependencyFlags.SoftDependency)]
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class STRAFTweakPlugin : BaseUnityPlugin
{
    internal static STRAFTweakPlugin Instance;

    Harmony _harmony = new(MyPluginInfo.PLUGIN_GUID);

    void Awake()
    {
        _harmony.PatchAll();
        Instance = this;
        this.gameObject.hideFlags = HideFlags.HideAndDontSave;

        SetOutlineColors.SetConfigBinds();
        ScrollJump.SetConfigBinds();
        Config.SettingChanged += ReloadConfig;

        var modMenuLoaded = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(ModMenu.PluginInfo.guid);
        // if (modMenuLoaded) STRAFTweakModMenuCompat.Start();
    }

    void OnDestroy()
    {
        Config.SettingChanged -= ReloadConfig;
        _harmony.UnpatchSelf();
    }

    void ReloadConfig(object sender, SettingChangedEventArgs args)
    {
        Config.Reload();

        if (args.ChangedSetting == SetOutlineColors.weaponColor
        || args.ChangedSetting == SetOutlineColors.weaponTextColor
        || args.ChangedSetting == SetOutlineColors.weaponTextBrightness)
            SetOutlineColors.UpdateColorsFromConfig();
        else if (args.ChangedSetting == ScrollJump.isEnabled)
            ScrollJump.Postfix();
    }

    // Debug
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.V))
    //     {
    //         var test = PlayerPrefs.GetString("PlayerControls (UnityEngine.InputSystem.InputActionAsset):PlayerChangeWeapon1");
    //         Debug.LogError(test);
    //     }
    // }
}

static class STRAFTweakModMenuCompat
{
    internal static void Start()
    {
        // Hot relaod
        // try
        // {
        //     ModMenu.Api.ModMenuCustomisation.SetPluginDescription("Tweak things like weapon outline color or mouse wheel binds :D\n\nChanges are applied immediately.");
        //     ModMenu.Api.ModMenuCustomisation.RegisterContentBuilder(ConfigBuilder);
        // }
        // catch (InvalidOperationException) { }
    }

    // static void ConfigBuilder(ModMenu.Api.OptionListContext c)
    // {
    // }
}