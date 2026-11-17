using HarmonyLib;
using StardewValley;
using Object = StardewValley.Object;
namespace CropInstantPickup;
[HarmonyPatch(typeof(Object), nameof(Object.cutWeed))]
internal static class WeedPatch
{
    private static void Prefix(Object __instance, out int __state)
    {
        __state = __instance.Location?.debris.Count ?? -1;
    }
    private static void Postfix(Object __instance, Farmer who, int __state)
    {
        if (!ModEntry.Config.InstantWeedPickup || __state < 0){return;}
        DebrisSweep.CollectNew(__instance.Location, who, __state);
    }
}