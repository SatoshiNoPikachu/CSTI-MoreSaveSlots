using HarmonyLib;

namespace MoreSaveSlots.Patcher;

[HarmonyPatch(typeof(GameLoad))]
public static class GameLoadPatch
{
    [HarmonyPrefix, HarmonyPatch("DeleteGameData")]
    public static bool DeleteGameData_Prefix(GameLoad __instance, int _Index)
    {
        SaveSlotCtrl.DeleteGameData(__instance, _Index);
        return false;
    }

    [HarmonyPostfix, HarmonyPatch("GenerateGameFileName")]
    public static void GenerateGameFileName_Postfix(ref string __result, int _Index)
    {
        __result = SaveSlotCtrl.GenerateGameFileName(_Index);
    }
}