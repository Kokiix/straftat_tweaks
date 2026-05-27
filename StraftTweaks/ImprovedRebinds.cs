
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ImprovedRebinds;

/// <summary>
/// Allow for key binds to swap, and scroll bind to jump
/// </summary>
[HarmonyPatch(typeof(InputManager), "DoRebind")]
static class ScrollBindFixer
{
    static void Prefix(InputAction actionToRebind, ref int bindingIndex, TextMeshProUGUI statusText, ref bool allCompositeParts, bool excludeMouse, bool sequenceDisplay)
    {
        Debug.LogError(actionToRebind);
        Debug.LogError(bindingIndex);

        Debug.LogError(actionToRebind.name);
        if (actionToRebind.name == "ChangeWeapon")
        {
            actionToRebind.Disable();
            actionToRebind.expectedControlType = "Button";

            // remove mouse bind
            // actionToRebind.bindings.Do(b => Debug.LogError(b.path));
            int scrollIndex = actionToRebind.bindings.IndexOf(b => b.path == "<Mouse>/scroll");
            if (scrollIndex != -1)
            {
                actionToRebind.ChangeBinding(scrollIndex).Erase();
            }

            // find/create composite bind
            var newBindIdx = actionToRebind.bindings.IndexOf(b => b.isPartOfComposite);
            if (newBindIdx == -1)
                newBindIdx = actionToRebind.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/T")
                .bindingIndex + 1;

            bindingIndex = newBindIdx;
        }
    }
}