using HarmonyLib;
using StardewValley;
using StardewValley.Objects;

namespace CropInstantPickup;
[HarmonyPatch(typeof(BreakableContainer), nameof(BreakableContainer.releaseContents))]
    internal static class BreakableContainerPatch
    {
        private static void Prefix(BreakableContainer __instance, out int __state)
        {
        __state = __instance.Location?.debris.Count ?? -1;
        }
        private static void Postfix(BreakableContainer __instance, Farmer who, int __state)
        {
        if (!ModEntry.Config.InstantBreakableContainerPickup || __state < 0){return;}
        DebrisSweep.CollectNew(__instance.Location, who, __state);
        }
    }
