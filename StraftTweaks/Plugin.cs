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