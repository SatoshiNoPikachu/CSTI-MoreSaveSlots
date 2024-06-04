using HarmonyLib;

namespace MoreSaveSlots.Patcher;

[HarmonyPatch(typeof(SaveMenu))]
public static class SaveMenuPatch
{
    [HarmonyPostfix, HarmonyPatch("OnEnable")]
    public static void OnEnable_Postfix(SaveMenu __instance)
    {
        SaveMenuCtrl.Create(__instance);
    }

    [HarmonyPostfix, HarmonyPatch("Refresh")]
    public static void Refresh_Postfix(SaveMenu __instance)
    {
        SaveMenuCtrl.OnRefresh(__instance);
    }
}