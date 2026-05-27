using BepInEx;
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

        var modMenuLoaded = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(ModMenu.PluginInfo.guid);
        if (modMenuLoaded) ModMenuCompat.Start();

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
    //         var test = PlayerPrefs.GetString("PlayerControls (UnityEngine.InputSystem.InputActionAsset):PlayerChangeWeapon1");
    //         Debug.LogError(test);
    //     }
    // }
}

static class ModMenuCompat
{
    internal static void Start()
    {
        ModMenu.Api.ModMenuCustomisation.SetPluginDescription("Tweak things like weapon outline color or mouse wheel binds :D");

        SetOutlineColors.SetConfigBinds();
        ScrollJump.SetConfigBinds();

        ModMenu.Api.ModMenuCustomisation.HideEntry(ScrollJump.isEnabled);
        ModMenu.Api.ModMenuCustomisation.RegisterContentBuilder(ConfigBuilder);
    }

    static void ConfigBuilder(ModMenu.Api.OptionListContext c)
    {
        c.InsertButton(4, "Apply Changes", "Apply Changes", SetOutlineColors.UpdateColorsFromConfig);

        c.AppendHeader("Bindings");
        c.AppendCheckbox("Scroll to jump", () => ScrollJump.isEnabled.Value, ScrollJump.SetValue);
    }
}