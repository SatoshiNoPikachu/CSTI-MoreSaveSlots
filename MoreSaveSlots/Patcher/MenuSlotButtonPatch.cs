using HarmonyLib;

namespace MoreSaveSlots.Patcher;

[HarmonyPatch(typeof(MenuSlotButton))]
public static class MenuSlotButtonPatch
{
    [HarmonyPrefix, HarmonyPatch("Setup")]
    public static void Setup_Prefix(MenuSlotButton __instance)
    {
        SaveSlotCtrl.OnMenuButtonSetup(__instance);
    }
}