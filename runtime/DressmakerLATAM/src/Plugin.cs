using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DressmakerLATAM;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public sealed class Plugin : BaseUnityPlugin
{
    internal const string PluginGuid = "com.dressmaker.latam.localization";
    internal const string PluginName = "Dressmaker LATAM";
    internal const string PluginVersion = "0.1.1";

    internal const string LocaleCode = "es-419";
    internal const string LocaleDisplayName = "Español (Latinoamérica)";

    private static readonly string[] KnownTables = { "Content", "Dialogue", "Tutorial", "UI" };
    private static readonly Dictionary<string, Dictionary<long, string>> Tables = new(StringComparer.Ordinal);

    private static ManualLogSource? Log;
    private static bool registered;

    private void Awake()
    {
        Log = Logger;
        LoadCsvFiles(Path.GetDirectoryName(Info.Location) ?? ".");
        Log.LogInfo($"{PluginName} {PluginVersion} loaded. Waiting for Unity Localization initialization.");
    }

    private void Update()
    {
        if (registered)
        {
            return;
        }

        AsyncOperationHandle<LocalizationSettings> initialization = LocalizationSettings.InitializationOperation;
        if (!initialization.IsDone)
        {
            return;
        }

        RegisterLocale();
    }

    private static void LoadCsvFiles(string pluginDirectory)
    {
        foreach (string table in KnownTables)
        {
            string path = Path.Combine(pluginDirectory, table + ".csv");
            if (!File.Exists(path))
            {
                Log?.LogWarning($"{table}.csv not found; {table} will fall back to English.");
                continue;
            }

            Dictionary<long, string> entries = RuntimeCsv.Load(path, Log);
            Tables[table] = entries;
            Log?.LogInfo($"{table}.csv loaded: {entries.Count} translations.");
        }
    }

    private static void RegisterLocale()
    {
        if (registered)
        {
            return;
        }

        registered = true;

        ILocalesProvider locales = LocalizationSettings.AvailableLocales;
        Locale locale = locales.GetLocale(LocaleCode);
        if (locale == null)
        {
            locale = Locale.CreateLocale(new LocaleIdentifier(LocaleCode));
            locale.name = LocaleDisplayName;
            locales.AddLocale(locale);
            Log?.LogInfo($"Registered locale {LocaleCode} as '{LocaleDisplayName}'.");
        }
        else
        {
            locale.name = LocaleDisplayName;
            Log?.LogInfo($"Using existing locale {LocaleCode}; display name set to '{LocaleDisplayName}'.");
        }

        LocalizationSettings.StringDatabase.TableProvider =
            new LatamTableProvider(LocalizationSettings.StringDatabase.TableProvider, Tables, Log);
        LocalizationSettings.AssetDatabase.TableProvider =
            new LatamTableProvider(LocalizationSettings.AssetDatabase.TableProvider, Tables, Log);

        Log?.LogInfo("Dressmaker LATAM table provider registered.");
    }
}
