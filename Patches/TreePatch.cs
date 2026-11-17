
using HarmonyLib;
using StardewValley;
using StardewValley.TerrainFeatures;

namespace CropInstantPickup;
[HarmonyPatch(typeof(Tree), nameof(Tree.tickUpdate))]
internal static class TreePatch
{
    private static void Prefix(Tree __instance, out int __state)
    {
        __state = __instance.falling.Value ? __instance.Location?.debris.Count ?? -1 : -1;
    }
    private static void Postfix(Tree __instance, int __state)
    {
        if (!ModEntry.Config.InstantTreePickup ||  __state < 0)
        {
            return;
        }
        //我的客户端给别人背包加这不扯淡吗
        //如果这里是被炸了，那么就判断成主机，那就只有主机收喽？如果主机没下这个mod就变成普通掉落物
        Farmer who = Game1.GetPlayer(__instance.lastPlayerToHit.Value) ?? Game1.MasterPlayer;
            DebrisSweep.CollectNew(__instance.Location, who, __state);
    }
}

[HarmonyPatch(typeof(Tree), nameof(Tree.performToolAction))]
internal static class TreeChoppingPatch
{
    private static void Prefix(Tree __instance, out int __state)
    {
        __state = __instance.Location?.debris.Count ?? -1;
    }
    private static void Postfix(Tree __instance, Tool t, int __state)
    {
        if (!ModEntry.Config.InstantTreePickup || __state < 0)
        {
           return; 
        }    
        DebrisSweep.CollectNew(__instance.Location, t?.getLastFarmerToUse(), __state);
    }
}