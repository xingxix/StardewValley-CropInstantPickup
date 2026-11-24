using System;
using System.Collections.Generic;
using HarmonyLib;
using StardewModdingAPI.Events;
using StardewValley;

namespace CropInstantPickup;
[HarmonyPatch(typeof(GameLocation), nameof(GameLocation.explode))]
internal static class ExplodeTracker
{
    //原来你不是为了捡东西，是为了干掉东西。。。
    //那你会漏记吗。。？
    //哈哈，会呢
    internal static HashSet<Debris> Marker = new HashSet<Debris>();
    private static void Prefix(GameLocation __instance, out HashSet<Debris> __state)
    {
        __state = null;
        if(MonsterPatch.KillDepth > 0){
        __state = new HashSet<Debris>(__instance.debris);
        }
    }
    private static void Postfix(GameLocation __instance, HashSet<Debris> __state)
    {
        if (__state is null){return;}
        for (int i = __instance.debris.Count - 1; i >= 0; --i)
        {
            if(__state.Contains(__instance.debris[i])){break;}
            Marker.Add(__instance.debris[i]);

        }
    }
}
//爆炸会删伤害数字，伤害数字也是Debris。？

//明白了，你认为onMonsterKilled在爆炸后面，所以不会有问题，实则不然
//爆炸产生在OnMonsterKilled里面，你看到的那个在后面的是另一个OnMonsterKilled，别人家的好吧
[HarmonyPatch(typeof(GameLocation), "onMonsterKilled")]
internal static class MonsterPatch
{
    internal static int KillDepth = 0;
    // private static void Prefix(GameLocation __instance, out int __state)
    // {
    //     __state = -1;
    //     if(!ModEntry.Config.InstantMonsterPickup){return;}
    //     isKilling = true;
    //     __state = __instance.debris.Count;
    // }
    private static void Prefix(GameLocation __instance, out HashSet<Debris> __state)
    {
        __state = null;
        if(!ModEntry.Config.InstantMonsterPickup){return;}
        ++KillDepth;
        __state = new HashSet<Debris>(__instance.debris);
    }
    private static void Postfix(GameLocation __instance, Farmer who, HashSet<Debris> __state)
    {
    if (!ModEntry.Config.InstantMonsterPickup || __state is null) {return;}
    DebrisSweep.CollectNew(__instance, who, __state, ExplodeTracker.Marker);

    }
    private static Exception Finalizer(Exception __exception)
    {
        if(ModEntry.Config.InstantMonsterPickup){
        --KillDepth;
        if(KillDepth == 0){ExplodeTracker.Marker.Clear();}
        }
        return __exception;
    }
}


internal static class FTMonsterPatch
{
 internal static void SaveNum(NpcListChangedEventArgs e, out int __state)
    {        
        __state = e.Location?.debris.Count ?? -1;
        
    }   
internal static void CollectThem(NpcListChangedEventArgs e, int __state)
    {
        if (!ModEntry.Config.InstantFTMPickup || __state < 0)
        {
            return;
        }   
        if(Game1.IsMasterGame)
        {
         DebrisSweep.CollectNew(e.Location, Game1.player, __state);
        }
   
    }
}
