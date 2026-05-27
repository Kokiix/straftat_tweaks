
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using BepInEx;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ImprovedRebinds;

enum BindingType
{
    Keypress,
    Scroll,
}

static class Util
{
    internal static void SetBindingType(BindingType type)
    {
        var actionToRebind = InputManager.inputActions.asset.FindAction("ChangeWeapon");
        if (type == BindingType.Keypress)
        {
            actionToRebind.AddCompositeBinding("2DVector").With("Up", "<Keyboard>/T");
        }
        else
        {
            actionToRebind.AddBinding("<Mouse>/scroll");
        }
        actionToRebind.ChangeBinding(0).Erase();
    }
}

[HarmonyPatch(typeof(InputManager), "StartRebind")]
static class AllowBindToComposite
{
    static void Prefix(string actionName, int bindingIndex, TextMeshProUGUI statusText, bool excludeMouse, bool sequenceDisplay)
    {
        if (actionName == "ChangeWeapon")
        {
            Util.SetBindingType(BindingType.Keypress);
        }
    }

}

[HarmonyPatch(typeof(InputManager), "DoRebind")]
static class StopCompositeMultiBind
{
    static void Prefix(InputAction actionToRebind, ref bool allCompositeParts)
    {
        if (actionToRebind.name == "ChangeWeapon")
        {
            allCompositeParts = false;
        }
    }
}

[HarmonyPatch(typeof(InputManager), "ResetBinding")]
static class ResetBinding
{
    static void Postfix(string actionName)
    {
        Util.SetBindingType(BindingType.Scroll);
        InputAction inputAction = InputManager.inputActions.asset.FindAction(actionName);
        inputAction.bindings.Do(b => Debug.LogError(b));
    }
}

[HarmonyPatch(typeof(InputManager), "LoadBindingOverride")]
static class ApplyOverride
{
    static void Prefix(string actionName)
    {
        var overrideString = PlayerPrefs.GetString("PlayerControls (UnityEngine.InputSystem.InputActionAsset):PlayerChangeWeapon1");
        if (overrideString.IsNullOrWhiteSpace()) return;

        Util.SetBindingType(BindingType.Keypress);
    }
}


// [HarmonyPatch(typeof(InputManager), "SaveBindingOverride")]
// static class Test
// {
//     static void Postfix(InputAction action)
//     {
//         for (int i = 0; i < action.bindings.Count; i++)
//         {
//             var test = action.actionMap?.ToString() + action.name + i;
//             Debug.LogError(test.Length);
//         }
//     }
// }