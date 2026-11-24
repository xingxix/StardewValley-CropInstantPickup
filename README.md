# -mod-CropInstantPickup
星露谷mod，虽然叫农作物但现在已经是立刻拾取各种物品了呢（

还有高手？怎么茶树苗不算果树啊？你又是走的哪个函数？再探，再报！

难绷，还有巨大作物也不算作物的，神了

也不算砍树啊！神了

我去，我忘了原来还有个bush

难绷，怎么灵魂精华也不能，是因为任务期间掉落不走杀怪掉落而是单独判断吗，但是不走杀怪掉落你这是要在哪判断呢

还有砍树额外掉落木头也捡不起来，可能要看一下performtoolaction

割草掉的纤维和种子（？）没有提示是何意味、

开了砍树立刻拾取之后仍然不能拾取小木头是何意味

  1. Bush 系（茶叶 / 茶苗 / 核桃 / 浆果）
  - Bush.performUseAction(Vector2) — 右键摇：茶叶 (O)815（createObjectDebris 422）、浆果 (O)296/410（434）
  - Bush.performToolAction(Tool, int, Vector2) — 斧头/炸弹：茶叶 + 茶苗 (O)251（619）
  - 坑：核桃 size 4 的掉落走 NetMutex.RequestLock 回调（NetMutex.cs:44-59），回调要等 Update 里才执行（78-90），是异步的
   → 单纯 Postfix 扫不到，得再挂 Bush.tickUpdate 或延迟一帧补扫
  - 开关：建议新加 InstantBushPickup；也可以整组并进果树开关

  2. 巨大作物
  - GiantCrop.performToolAction(Tool, int, Vector2) — 主掉落、秘密纸条、碎屑附魔都在里面
  - 开关：挂 InstantResourceClumpPickup（它本来就是 ResourceClump 子类）

  3. 砍果树（顺带）
  - FruitTree.performToolAction(Tool, int, Vector2) — 树桩的木材/树液/树苗/果子（838/842/848，落果 729-767）
  - FruitTree.tickUpdate(GameTime) — 倒下时（falling.Value）的木材/树液（300-324）
  - 开关：InstantTreePickup

  4. 嫩枝（地上的 Twig）
  - Object.performToolAction(Tool) — 现有 BeachFarmSuppliesPatch 已经挂在这个方法上，可以在同一个门控里加 IsTwig() && t
  is Axe
  - Object.onExplosion(Farmer) — 炸弹炸嫩枝出木材（1614）
  - 开关：InstantTreePickup

  5. 灵魂精华（FTM）
  没有原版类可挂，两个方案：
  - A（推荐）：自己订阅 World.NpcListChanged，发现移除的是 Monster 就记下 location，下一 tick 再 sweep 一次。注意我们比
  FTM 先加载，事件里我们会比 FTM 先执行，所以当场扫不到，必须延迟
  - B：Harmony Postfix 挂 FarmTypeManager.ModEntry.NpcListChanged，在 FTM 掉落后扫。精准，但依赖它私有方法名
  - 开关：InstantMonsterPickup

  6. 你说的其他 mod 的额外木头
  得先知道 mod 名。如果它是在 Tree.performToolAction 的 Harmony Postfix 里加掉落，我们同方法的 Postfix 可能比它先跑 →
  把我们的扫描调成低优先级（HarmonyPriority/HarmonyAfter）或者延迟一帧就能收到；如果是它自己的方法，就只能针对它单独
  hook。

  新增开关的话，ModConfig、GMCM、i18n（zh + default）三处都要跟着补。上面每一项都是 Prefix 记 debris.Count、Postfix 调
  DebrisSweep.CollectNew 的老套路，可以直接复用。