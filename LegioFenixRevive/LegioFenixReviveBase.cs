using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace LegioFenixRevive;

[BepInPlugin("legiofenix.blackal.LegioFenixRevive", "LegioFenixRevive", "1.0")]
public class LegioFenixReviveBase : BaseUnityPlugin
{
    public static readonly Harmony harmony = new Harmony("LegioFenixRevive");

    internal static LegioFenixReviveBase Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger => Instance._logger;
    private ManualLogSource _logger => base.Logger;

    public static ConfigEntry<int> minHealthRequiredToRevive { get; private set; } = null!;
    public static ConfigEntry<float> percentOfHealthToTransfer { get; private set; } = null!;

    private void Awake()
    {
        Instance = this;

        // Prevent the plugin from being deleted  
        this.gameObject.transform.parent = null;
        this.gameObject.hideFlags = HideFlags.HideAndDontSave;

        LoadConfig();
        harmony.PatchAll();

        Logger.LogInfo($"   {Info.Metadata.GUID} v{Info.Metadata.Version} has loaded!");
    }

    private void LoadConfig()
    {
        minHealthRequiredToRevive = base.Config.Bind<int>("General", "minHealthRequiredToRevive", 50, "Minimum health required to do a revive on someone.");
        percentOfHealthToTransfer = base.Config.Bind<float>("General", "percentOfHealthToTransfer", 0.5f, "Percentage of revivers health that will be transferred to the person being revived.");
    }
}
