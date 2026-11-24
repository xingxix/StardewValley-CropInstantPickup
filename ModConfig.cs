namespace CropInstantPickup;

public sealed class ModConfig
{
    //农作物
    public bool InstantCropPickup {get; set;} = true;
    //果树
    public bool InstantFruitTreesPickup { get; set; } = true;
    //挖矿
    public bool InstantMiningPickup {get; set;} = false;
    //杂草
    public bool InstantWeedPickup {get; set;} = false;
    //大石头
    public bool InstantResourceClumpPickup {get; set;} = false;
    //木桶子
    public bool InstantBreakableContainerPickup {get; set;} = false;
    //补给箱
    public bool InstantSupplyCratePickup {get; set;} = false;
    //砍树
    public bool InstantTreePickup {get; set;} = false;
    //怪物掉落
    public bool InstantMonsterPickup {get; set;} = false;
    //灌木类
    public bool InstantBushPickup {get; set;} = false;

    //FTM
    public bool InstantFTMPickup {get; set;} = false;
}
