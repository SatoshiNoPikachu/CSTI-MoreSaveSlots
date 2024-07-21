using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace MoreSaveSlots.Patcher;

[HarmonyPatch(typeof(GameManager))]
public static class GameManagerPatch
{
    [HarmonyPostfix, HarmonyPatch("GetSavedLogs", MethodType.Getter)]
    public static void GetSavedLogs_Postfix(ref List<LogSaveData> __result)
    {
        __result = __result.ToList();
    }
}