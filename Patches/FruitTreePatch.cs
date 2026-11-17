using HarmonyLib;
using StardewValley;
using StardewValley.TerrainFeatures;

namespace CropInstantPickup;

[HarmonyPatch(typeof(FruitTree), nameof(FruitTree.performUseAction))]
internal static class FruitTreeActionPatch
{
    private static void Prefix(FruitTree __instance, out int __state)
    {
        __state = __instance?.Location?.debris.Count ?? -1;
    }

    private static void Postfix(FruitTree __instance, int __state)
    {
        if (!ModEntry.Config.InstantFruitTreesPickup || __state < 0)
            return;

        GameLocation location = __instance?.Location;
        if (location == null)
            return;

        // for (int i = __state; i < location.debris.Count; i++)
        // {
        //     Debris debris = location.debris[i];
        //     if (debris?.item == null)
        //         continue;

        //     if (Game1.player.addItemToInventoryBool(debris.item))
        //     {
        //         location.debris.RemoveAt(i);
        //         i--;
        //     }
        // }
        DebrisSweep.CollectNew(location, Game1.player, __state);
    }
}
