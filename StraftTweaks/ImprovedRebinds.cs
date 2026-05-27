
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
        if (actionName != "ChangeWeapon") return;
        InputAction inputAction = InputManager.inputActions.asset.FindAction(actionName);
        inputAction.bindings.Do(b => Debug.LogError(b));

        // if (inputAction.bindings.Count == 1)
        // {
        //     inputAction.ChangeBinding(0).Erase();
        //     inputAction.AddCompositeBinding("2DVector").With("Up", "");
        //     if (PlayerPrefs.GetString("PlayerControls (UnityEngine.InputSystem.InputActionAsset):PlayerChangeWeapon1").IsNullOrWhiteSpace())
        //         inputAction.AddBinding("<Mouse>/scroll");
        // }
    }
}

[HarmonyPatch(typeof(InputManager), "DoRebind")]
static class OnBind
{
    static void Prefix(InputAction actionToRebind, ref bool allCompositeParts)
    {
        if (actionToRebind.name != "ChangeWeapon")
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

[HarmonyPatch(typeof(InputManager), "LoadBindingOverride")]
static class ScrollJump
{
    internal static bool isEnabled;

    internal static void SetBind()
    {
        isEnabled = STRAFTweakPlugin.Instance.Config.Bind("Binding", "Scroll to jump", false).Value;
    }

    static void Postfix()
    {
        SetBind();
        SetValue(isEnabled);
    }

    internal static void SetValue(bool scrolljump)
    {
        var inputAction = InputManager.inputActions.asset.FindAction("Jump");
        inputAction.bindings.Do(b => Debug.LogError(b));
        // if (scrolljump)
        // {
        //     if (inputAction.bindings.Count > 1) return;
        //     inputAction.AddBinding("<Mouse>/scroll");
        // }
        // else
        // {
        //     if (inputAction.bindings.Count == 1) return;
        //     inputAction.ChangeBinding(1).Erase();
        // }
    }
}

static class ModMenuCompat
{
    internal static void Start()
    {
        ModMenu.Api.ModMenuCustomisation.RegisterContentBuilder(ConfigBuilder);
        ScrollJump.SetBind();
    }

    static void ConfigBuilder(ModMenu.Api.OptionListContext c)
    {
        c.AppendHeader("Bindings");
        c.AppendCheckbox("Scroll to jump", () => ScrollJump.isEnabled, ScrollJump.SetValue);
    }
}