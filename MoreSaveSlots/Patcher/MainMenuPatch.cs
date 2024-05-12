using HarmonyLib;

namespace MoreSaveSlots.Patcher;

[HarmonyPatch(typeof(MainMenu))]
public static class MainMenuPatch
{
    [HarmonyPostfix, HarmonyPatch("Awake")]
    public static void Awake_Postfix(MainMenu __instance)
    {
        SaveSlotCtrl.Create(__instance);
    }

    [HarmonyPrefix, HarmonyPatch("SetupSlotsScreen")]
    public static bool SetupSlotsScreen_Prefix()
    {
        SaveSlotCtrl.Instance?.ChangePage();
        return false;
    }

    [HarmonyPrefix, HarmonyPatch("DeleteSlot")]
    public static bool DeleteSlot_Prefix(MainMenu __instance, int _Index)
    {
        SaveSlotCtrl.DeleteSlot(__instance, _Index);
        return false;
    }
}