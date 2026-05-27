
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using HarmonyLib;
using UnityEngine;

namespace ImprovedRebinds;

[HarmonyPatch(typeof(InputManager), "Awake")]
static class Test
{
    static void Postfix()
    {
        Debug.LogError("test");
    }
}

// static class ModMenu
// {
//     [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
//     internal static void ConfigBuilder(OptionListContext c)
//     {
//         // c.AppendButton("Apply Changes", "Apply Changes", SetOutlineColors.UpdateColorsFromConfig);
//     }
// }