using System.Runtime.CompilerServices;
using HarmonyLib;
using StardewValley;
using StardewValley.Tools;

namespace CropInstantPickup;
[HarmonyPatch(typeof(GameLocation), nameof(GameLocation.OnStoneDestroyed))]
internal static class OrePatch{    
//目标是记录当前掉落物数量，一会遍历用
private static void Prefix(GameLocation __instance, out int __state){
        __state = __instance.debris.Count;
    }
private static void Postfix(GameLocation __instance, Farmer who, int __state){
        if (!ModEntry.Config.InstantMiningPickup)
        {
            return;
        }
        // for (int i = __state; i < __instance.debris.Count; ++i){
        //     Debris debris = __instance.debris[i];
        //     if(debris.item == null && string.IsNullOrEmpty(debris.itemId.Value))
        //     {
        //         continue;
        //     }
        //     if (debris.collect(who))
        //     {
        //      __instance.debris.RemoveAt(i);
        //      --i;   
        //     }
        // }
        DebrisSweep.CollectNew(__instance, who, __state);
    }
}

[HarmonyPatch(typeof(Pickaxe), nameof(Pickaxe.DoFunction))]
internal static class MiningDiamondPatch
{
    private static void Prefix(GameLocation location, Farmer who, out int __state)
    {
        __state = -1;
        if(who != null && who.stats.Get("Book_Diamonds") != 0)
        {
        __state = location?.debris.Count ?? -1;
        }
    }
    private static void Postfix(GameLocation location, Farmer who, int __state)
    {
        if (!ModEntry.Config.InstantMiningPickup || __state < 0){return;}
        DebrisSweep.CollectNew(location, who, __state, "(O)72");
    }
}
// [HarmonyPatch(typeof(Pickaxe), nameof(Pickaxe.DoFunction))]
// internal static class MiningDiamondPatch
// {
    
// }