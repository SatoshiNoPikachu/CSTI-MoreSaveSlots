using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace MoreSaveSlots;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "Pikachu.CSTIMod.MoreSaveSlots";
    public const string PluginName = "MoreSaveSlots";
    public const string PluginVersion = "1.0.1";

    internal static ManualLogSource Log;
    private static readonly Harmony Harmony = new(PluginGuid);

    private void Awake()
    {
        Harmony.PatchAll();

        Log = Logger;
        Log.LogInfo($"Plugin {PluginName} is loaded!");
    }
}