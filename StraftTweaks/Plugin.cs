using BepInEx;
using ComputerysModdingUtilities;
using HarmonyLib;
using UnityEngine;

[assembly: StraftatMod(isVanillaCompatible: true)]

[BepInPlugin("com.koki.tweaks", "STRAFTAT Tweaks", "1.0.0")]
public class KokiWeaponsPlugin : BaseUnityPlugin
{
    internal static Harmony Harmony;

    private void Awake()
    {
        this.gameObject.hideFlags = HideFlags.HideAndDontSave;

        var shader = Shader.Find("S_WeaponOutline_00");
        Debug.LogError(shader);
    }

    private void OnDestroy()
    {

    }
}