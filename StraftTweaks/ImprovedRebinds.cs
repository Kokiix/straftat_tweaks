
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ImprovedRebinds;

static class Util
{
    internal static void ReplaceVectorWithComposite(string actionName)
    {
        var actionToRebind = InputManager.inputActions.asset.FindAction(actionName);
        if (actionToRebind.bindings[0].path == "<Mouse>/scroll")
        {
            actionToRebind.ChangeBinding(0).Erase();
            actionToRebind.AddCompositeBinding("2DVector").With("Up", "<Keyboard>/T");
        }
    }
}

[HarmonyPatch(typeof(InputManager), "StartRebind")]
static class AllowBindToComposite
{
    static void Prefix(string actionName, int bindingIndex, TextMeshProUGUI statusText, bool excludeMouse, bool sequenceDisplay)
    {
        if (actionName == "ChangeWeapon")
        {
            Util.ReplaceVectorWithComposite(actionName);
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