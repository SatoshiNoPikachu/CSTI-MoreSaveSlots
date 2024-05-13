using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace MoreSaveSlots;

public static class Localization
{
    private const string KeyPrefix = $"{Plugin.PluginName}_";
    public const string KeyPreviousPage = $"{KeyPrefix}PreviousPage";
    public const string KeyNextPage = $"{KeyPrefix}NextPage";

    public static void LoadLanguage()
    {
        var manager = LocalizationManager.Instance;
        if (manager is null) return;

        var currentLanguage = LocalizationManager.CurrentLanguage;
        if (currentLanguage < 0 || currentLanguage >= manager.Languages.Length)
            return;

        var path = GetFilePath(manager.Languages[currentLanguage]);
        if (!File.Exists(path)) return;

        var text = File.ReadAllText(path);
        var localizationDict = Parse(text);

        if (localizationDict is null) return;
        var texts = LocalizationManager.CurrentTexts;
        var regex = new Regex(@"\\n");
        foreach (var (key, value) in localizationDict)
        {
            if (value.Count < 1) continue;
            texts[$"{KeyPrefix}{key}"] = regex.Replace(value[0], "\n");
        }
    }

    private static string GetFilePath(LanguageSetting setting)
    {
        var fileName = Path.GetFileName(setting.FilePath);
        if (fileName == "") fileName = "En.csv";
        return Path.Combine(Plugin.PluginPath, "Localization", fileName);
    }

    private static Dictionary<string, List<string>> Parse(string text)
    {
        try
        {
            return CSVParser.LoadFromString(text);
        }
        catch (Exception e)
        {
            Plugin.Log.LogError(e);
            return null;
        }
    }
}