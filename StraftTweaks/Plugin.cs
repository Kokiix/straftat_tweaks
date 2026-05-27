using BepInEx;
using ComputerysModdingUtilities;
using HarmonyLib;
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
        if (modMenuLoaded) WeaponOutlineColors.ModMenuCompat.Start();

    }

    void OnDestroy()
    {
        _harmony.UnpatchSelf();
    }

    // Debug
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            var test = PlayerPrefs.GetString("PlayerControls (UnityEngine.InputSystem.InputActionAsset):PlayerChangeWeapon1");
            Debug.LogError(test);
        }
    }
}