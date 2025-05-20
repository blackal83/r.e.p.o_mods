using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace LegioFenixRevive;

[BepInPlugin("legiofenix.blackal.LegioFenixRevive", "LegioFenixRevive", "0.1")]
public class LegioFenixReviveBase : BaseUnityPlugin
{
    private const string pluginGUID = "legiofenix.blackal.LegioFenixRevive";

    private const string pluginName = "LegioFenixRevive";

    private const string pluginVersion = "0.1";

    public static readonly Harmony harmony = new Harmony("LegioFenixRevive");

    internal static LegioFenixReviveBase Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger => Instance._logger;
    private ManualLogSource _logger => base.Logger;

    public static ConfigEntry<int> healthToTransfer;



    private void Awake()
    {
        Instance = this;
        
        // Prevent the plugin from being deleted
        this.gameObject.transform.parent = null;
        this.gameObject.hideFlags = HideFlags.HideAndDontSave;

        LoadConfig();
        harmony.PatchAll();

        Logger.LogInfo($"{Info.Metadata.GUID} v{Info.Metadata.Version} has loaded!");
    }


    private void LoadConfig()
    {
        LegioFenixReviveBase.healthToTransfer = base.Config.Bind<int>("General", "healthToTransfer", 50, "Health that will be moved from the reviver to the revived.  This value + 1 is the minimum health required to revive someone.");
    }
}