using HarmonyLib;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace CropInstantPickup;

public class ModEntry : Mod
{
    internal static ModConfig Config;

    public override void Entry(IModHelper helper)
    {
        Config = helper.ReadConfig<ModConfig>();

        new Harmony(ModManifest.UniqueID).PatchAll();

        helper.Events.GameLoop.GameLaunched += OnGameLaunched;
    }

    private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
    {
        IGenericModConfigMenuApi api = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
        if (api == null)
            return;

        api.Register(
            ModManifest,
            reset: () => Config = new ModConfig(),
            save: () => Helper.WriteConfig(Config)
        );
        
        api.AddBoolOption(
            ModManifest,
            getValue: () => Config.InstantCropPickup,
            setValue: value => Config.InstantCropPickup = value,
            name: () => Helper.Translation.Get("config.instant-crop-pickup.name"),
            tooltip: () => Helper.Translation.Get("config.instant-crop-pickup.tooltip")
        );
        api.AddBoolOption(
            ModManifest,
            getValue: () => Config.InstantFruitTreesPickup,
            setValue: value => Config.InstantFruitTreesPickup = value,
            name: () => Helper.Translation.Get("config.instant-fruit-trees-pickup.name"),
            tooltip: () => Helper.Translation.Get("config.instant-fruit-trees-pickup.tooltip")
        );
        api.AddBoolOption(
            ModManifest,
            getValue: () => Config.InstantMiningPickup,
            setValue: value => Config.InstantMiningPickup = value,
            name: () => Helper.Translation.Get("config.instant-mining-pickup.name"),
            tooltip: () => Helper.Translation.Get("config.instant-mining-pickup.tooltip")
        );
        api.AddBoolOption(
            ModManifest,
            getValue: () => Config.InstantWeedPickup,
            setValue: value => Config.InstantWeedPickup = value,
            name: () => Helper.Translation.Get("config.instant-weed-pickup.name"),
            tooltip: () => Helper.Translation.Get("config.instant-weed-pickup.tooltip")
        );
        api.AddBoolOption(
            ModManifest,
            getValue: () => Config.InstantResourceClumpPickup,
            setValue: value => Config.InstantResourceClumpPickup = value,
            name: () => Helper.Translation.Get("config.instant-resourceclump-pickup.name"),
            tooltip: () => Helper.Translation.Get("config.instant-resourceclump-pickup.tooltip")
        );
        api.AddBoolOption(
            ModManifest,
            getValue: () => Config.InstantBreakableContainerPickup,
            setValue: value => Config.InstantBreakableContainerPickup = value,
            name: () => Helper.Translation.Get("config.instant-breakablecontainer-pickup.name"),
            tooltip: () => Helper.Translation.Get("config.instant-breakablecontainer-pickup.tooltip")
        );

        api.AddBoolOption(
            ModManifest,
            getValue: () => Config.InstantTreePickup,
            setValue: value => Config.InstantTreePickup = value,
            name: () => Helper.Translation.Get("config.instant-tree-pickup.name"),
            tooltip: () => Helper.Translation.Get("config.instant-tree-pickup.tooltip")
        );
        api.AddBoolOption(
            ModManifest,
            getValue: () => Config.InstantMonsterPickup,
            setValue: value => Config.InstantMonsterPickup = value,
            name: () => Helper.Translation.Get("config.instant-monster-pickup.name"),
            tooltip: () => Helper.Translation.Get("config.instant-monster-pickup.tooltip")
        );
        api.AddSectionTitle(ModManifest, () => "杂项");
        api.AddBoolOption(
            ModManifest,
            getValue: () => Config.InstantSupplyCratePickup,
            setValue: value => Config.InstantSupplyCratePickup = value,
            name: () => Helper.Translation.Get("config.instant-supplycrate-pickup.name"),
            tooltip: () => Helper.Translation.Get("config.instant-supplycrate-pickup.tooltip")
        );
    }
}
