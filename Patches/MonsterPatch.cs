using System;
using System.Collections.Generic;
using HarmonyLib;
using StardewValley;

namespace CropInstantPickup;
[HarmonyPatch(typeof(GameLocation), nameof(GameLocation.explode))]
internal static class ExplodeTracker
{
    internal static HashSet<Debris> Marker = new HashSet<Debris>();
    private static void Prefix(GameLocation __instance, out int __state)
    {
        __state = -1;
        if(MonsterPatch.isKilling){
        Marker.Clear();
        __state = __instance.debris.Count;
        }
    }
    private static void Postfix(GameLocation __instance, int __state)
    {
        if (__state < 0){return;}
        for (int i = __state; i < __instance.debris.Count; ++i)
        {
            Marker.Add(__instance.debris[i]);

        }
    }
}


[HarmonyPatch(typeof(GameLocation), "onMonsterKilled")]
internal static class MonsterPatch
{
    internal static bool isKilling = false;
    private static void Prefix(GameLocation __instance, out int __state)
    {
        __state = -1;
        if(!ModEntry.Config.InstantMonsterPickup){return;}
        isKilling = true;
        __state = __instance.debris.Count;
    }
    private static void Postfix(GameLocation __instance, Farmer who, int __state)
    {    
    if (!ModEntry.Config.InstantMonsterPickup || __state < 0) {return;}
    DebrisSweep.CollectNew(__instance, who, __state, ExplodeTracker.Marker);

    }
    private static Exception Finalizer(Exception __exception)
    {
        isKilling = false;
        ExplodeTracker.Marker.Clear();
        return __exception;
    }
}