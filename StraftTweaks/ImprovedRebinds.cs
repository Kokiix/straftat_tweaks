
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using BepInEx;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ImprovedRebinds;

[HarmonyPatch(typeof(InputManager), "LoadBindingOverride")]
static class AddKeyPressInput
{
    static void Prefix(string actionName)
    {
        InputAction inputAction = InputManager.inputActions.asset.FindAction(actionName);
        if (inputAction.bindings.Count == 1)
        {
            inputAction.ChangeBinding(0).Erase();
            inputAction.AddCompositeBinding("2DVector").With("Up", "");
            if (PlayerPrefs.GetString("PlayerControls (UnityEngine.InputSystem.InputActionAsset):PlayerChangeWeapon1").IsNullOrWhiteSpace())
                inputAction.AddBinding("<Mouse>/scroll");
        }
    }
}

[HarmonyPatch(typeof(InputManager), "DoRebind")]
static class OnBind
{
    static void Prefix(InputAction actionToRebind, ref bool allCompositeParts)
    {
        if (actionToRebind.name == "ChangeWeapon")
        {
            if (actionToRebind.bindings.Count == 3)
                actionToRebind.ChangeBinding(2).Erase();
            actionToRebind.expectedControlType = "Button";
            allCompositeParts = false;
        }
    }
}

[HarmonyPatch(typeof(ReBindUI), "UpdateUI")]
static class UpdateBindUI
{
    static void Postfix(ReBindUI __instance)
    {
        var inputAction = __instance.inputActionReference.action;
        if (inputAction.name == "ChangeWeapon")
        {
            var overrideString = PlayerPrefs.GetString("PlayerControls (UnityEngine.InputSystem.InputActionAsset):PlayerChangeWeapon1");
            if (overrideString.IsNullOrWhiteSpace())
            {
                InputManager.inputActions.asset.FindAction("ChangeWeapon").AddBinding("<Mouse>/scroll");
                __instance.rebindText.text = "Scroll";
            }
            else
            {
                __instance.rebindText.text = overrideString[(overrideString.IndexOf("/") + 1)..].ToUpper();
            }
        }
    }
}