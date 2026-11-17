using System;
using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewValley;

namespace CropInstantPickup;

[HarmonyPatch(typeof(Crop), nameof(Crop.harvest))]
internal static class CropHarvestTrackerPatch
{
    [ThreadStatic]
    private static int _depth;

    internal static bool IsHarvesting => _depth > 0;

    private static void Prefix()
    {
        _depth++;
    }

    private static Exception Finalizer(Exception __exception)
    {
        _depth = Math.Max(0, _depth - 1);
        return __exception;
    }
}

[HarmonyPatch(
    typeof(Game1),
    nameof(Game1.createItemDebris),
    new[]
    {
        typeof(Item),
        typeof(Vector2),
        typeof(int),
        typeof(GameLocation),
        typeof(int),
        typeof(bool)
    }
)]
internal static class CropDebrisPatch
{
    private static bool Prefix(Item item)
    {
        if(!ModEntry.Config.InstantCropPickup){return true;}
        return !CropHarvestTrackerPatch.IsHarvesting
            || item == null
            || !Game1.player.addItemToInventoryBool(item);
    }
}
