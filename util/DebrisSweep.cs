using System.Collections.Generic;
using System.Runtime.CompilerServices;
using StardewValley;
namespace CropInstantPickup;

public static class DebrisSweep
{
    public static void CollectNew(GameLocation gameLocation, Farmer who, int index)
    {
        if(gameLocation == null || index < 0){
            return;
        }
        who ??= Game1.player; 
        //who是空的情况有
        // 怪物的爆炸，环境爆炸（有吗？）
        //踩碎绿雨灌木（还有这个？）
        //那么这两种情况下会降级成本地玩家，直接被本地玩家拾取可以吗？可以吧
        if(who.currentLocation != gameLocation){return;}//这个是防多人的时候主机不在地图莫名其妙收到东西
        if(who.UniqueMultiplayerID != Game1.player.UniqueMultiplayerID){return;}
        for(int i = index; i < gameLocation.debris.Count; ++i)
        {
            Debris debris = gameLocation.debris[i];
            if(debris.item == null && string.IsNullOrEmpty(debris.itemId.Value)){continue;}
            while(debris.Chunks.Count > 0 && debris.collect(who)){
                debris.Chunks.RemoveAt(0);
            }
            if (debris.Chunks.Count == 0)
            {
                gameLocation.debris.RemoveAt(i);
                --i;
            }
        }
    }
    public static void CollectNew(GameLocation gameLocation, Farmer who, int index, string itemId)
    {
        if(gameLocation is null || index < 0 || itemId is null){
            return;
        }
        who ??= Game1.player; 
        if(who.currentLocation != gameLocation){return;}
        if(who.UniqueMultiplayerID != Game1.player.UniqueMultiplayerID){return;}

        //下面才有修改
        for(int i = index; i < gameLocation.debris.Count; ++i)
        {
            Debris debris = gameLocation.debris[i];
            //因为要查id，所以不需要再判断空了，直接用两个字符串的比较
            //c#重载了string的=，如果都是字符串那直接比较值，不是就是正常的等，比较是否指向同一个
            //那粒子的null肯定不会和字符串等的，所以直接干掉了
            //如果传过来的itemId是null，那null和null会相等吗。。？
            //好吧，会
            // if(debris.item == null && string.IsNullOrEmpty(debris.itemId.Value)){continue;}
            if (debris.itemId.Value != itemId && debris.item?.QualifiedItemId != itemId) {continue;}
            while(debris.Chunks.Count > 0 && debris.collect(who)){
                debris.Chunks.RemoveAt(0);
            }
            if (debris.Chunks.Count == 0)
            {
                gameLocation.debris.RemoveAt(i);
                --i;
            }
        }
    }

    public static void CollectNew(GameLocation gameLocation, Farmer who, int index, ICollection<Debris> exclude)
    {
        if(gameLocation is null || index < 0 || exclude is null){
            return;
        }
        who ??= Game1.player; 
        if(who.currentLocation != gameLocation){return;}
        if(who.UniqueMultiplayerID != Game1.player.UniqueMultiplayerID){return;}

        //下面才有修改 x 2
        for(int i = index; i < gameLocation.debris.Count; ++i)
        {
            Debris debris = gameLocation.debris[i];
            if(debris.item == null && string.IsNullOrEmpty(debris.itemId.Value)){continue;}
            //不成功，便成false，成false就继续
            if (exclude.Remove(debris)){continue;}
            while(debris.Chunks.Count > 0 && debris.collect(who)){
                debris.Chunks.RemoveAt(0);
            }
            if (debris.Chunks.Count == 0)
            {
                gameLocation.debris.RemoveAt(i);
                --i;
            }
        }
    }
}