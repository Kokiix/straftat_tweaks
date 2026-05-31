using System;
using BepInEx;
using BepInEx.Configuration;
using ComputerysModdingUtilities;
using HarmonyLib;
using ImprovedRebinds;
using StraftTweaks;
using UnityEngine;
using HoverInfo;

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

        InitConfig();

        var modMenuLoaded = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(ModMenu.PluginInfo.guid);
        if (modMenuLoaded) STRAFTweakModMenuCompat.Start();
    }

    void InitConfig()
    {
        SetJoinMessage.message = Config.Bind("Custom Join Message", "Message", "{USER} has joined the lobby", "{USER} will be replaced with your username.");

        OutlineColor.weaponColor = Config.Bind("Outline Colors", "Weapon Outline Color", new Color(1, 0.8196079f, 0.427451f));
        OutlineColor.weaponTextColor = Config.Bind("Outline Colors", "Weapon Interact Text Color", new Color(2, 1.106f, 0));
        OutlineColor.weaponTextBrightness = Config.Bind("Outline Colors", "Weapon Interact Text Brightness", 1f);
        ApplyRainbow.enable = Config.Bind("Outline Colors", "Enable Rainbow Outlines", false);
        ApplyRainbow.rainbowSpeed = Config.Bind("Outline Colors", "Rainbow Fluctuation Speed", 0.2f, "i have no idea what the unit is for this");
        OutlineColor.weaponColor.SettingChanged += OutlineColor.UpdateColorsFromConfig;
        OutlineColor.weaponTextColor.SettingChanged += OutlineColor.UpdateColorsFromConfig;
        OutlineColor.weaponTextBrightness.SettingChanged += OutlineColor.UpdateColorsFromConfig;

        WeaponInteractPrompt.enablePrompt = Config.Bind("Hover Prompts", "Enable Weapon Interact Text", true);
        DoorInteractPrompt.enablePrompt = Config.Bind("Hover Prompts", "Enable Door Interact Text", true);
        WeaponInteractPrompt.disableKey = Config.Bind("Hover Prompts", "Disable Key Prompt", true, "Transform \"Handgun [F]\" into just \"Handgun\". Does the same for door interactions.");

        ScrollJump.isEnabled = Config.Bind("Binding", "Scroll to jump", false);
        ScrollJump.isEnabled.SettingChanged += (_, __) => ScrollJump.Postfix();
    }

    void OnDestroy()
    {
        _harmony.UnpatchSelf();
    }

    // Debug
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.V))
    //     {
    //         if (gameObject.TryGetComponent(out TestBehav test))
    //             UnityEngine.Object.Destroy(test);
    //         else
    //             gameObject.AddComponent<TestBehav>();
    //     }
    // }
}

static class STRAFTweakModMenuCompat
{
    internal static void Start()
    {
        // Hot relaod
        try
        {
            ModMenu.Api.ModMenuCustomisation.SetPluginDescription("Tweak things like weapon outline color or mouse wheel binds :D\n\nChanges are applied immediately.\n\n");
            // ModMenu.Api.ModMenuCustomisation.RegisterContentBuilder(ConfigBuilder);
        }
        catch (InvalidOperationException) { }
    }

    static void ConfigBuilder(ModMenu.Api.OptionListContext c)
    {
    }
}