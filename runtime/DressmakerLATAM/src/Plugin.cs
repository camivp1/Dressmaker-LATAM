using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
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
    internal const string PluginVersion = "0.1.2";

    internal const string LocaleCode = "es-419";
    internal const string LocaleDisplayName = "Español (Latinoamérica)";

    private static readonly string[] KnownTables = { "Content", "Dialogue", "Tutorial", "UI" };
    private static readonly Dictionary<string, Dictionary<long, string>> Tables = new(StringComparer.Ordinal);

    private static ManualLogSource? Log;
    private static bool registered;
    private static bool providersRegistered;
    private static Harmony? harmony;

    private void Awake()
    {
        Log = Logger;
        LoadCsvFiles(Path.GetDirectoryName(Info.Location) ?? ".");
        RegisterLocale();
        InstallLocaleDisplayPatch();
        Log.LogInfo($"{PluginName} {PluginVersion} loaded. Waiting for Unity Localization initialization.");
    }

    private void Update()
    {
        if (!registered)
        {
            RegisterLocale();
        }

        if (providersRegistered)
        {
            return;
        }

        if (!registered)
        {
            return;
        }

        AsyncOperationHandle<LocalizationSettings> initialization = LocalizationSettings.InitializationOperation;
        if (!initialization.IsDone)
        {
            return;
        }

        RegisterProviders();
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

        try
        {
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

            registered = true;
        }
        catch (Exception ex)
        {
            Log?.LogWarning($"Could not register locale {LocaleCode} yet: {ex.GetType().Name}: {ex.Message}");
        }
    }

    private static void RegisterProviders()
    {
        if (providersRegistered)
        {
            return;
        }

        providersRegistered = true;
        LocalizationSettings.StringDatabase.TableProvider =
            new LatamTableProvider(LocalizationSettings.StringDatabase.TableProvider, Tables, Log);
        LocalizationSettings.AssetDatabase.TableProvider =
            new LatamTableProvider(LocalizationSettings.AssetDatabase.TableProvider, Tables, Log);

        Log?.LogInfo("Dressmaker LATAM table provider registered.");
    }

    private static void InstallLocaleDisplayPatch()
    {
        try
        {
            Type? selectorType = AccessTools.TypeByName("LanguageSelectorPopup");
            MethodInfo? nativeName = selectorType == null ? null : AccessTools.Method(selectorType, "NativeName");
            if (nativeName == null)
            {
                Log?.LogWarning("LanguageSelectorPopup.NativeName was not found; the locale may display its system name.");
                return;
            }

            harmony = new Harmony(PluginGuid + ".locale-display");
            harmony.Patch(nativeName, postfix: new HarmonyMethod(typeof(Plugin), nameof(OverrideLocaleNativeName)));
            Log?.LogInfo("Patched LanguageSelectorPopup.NativeName for es-419 display name.");
        }
        catch (Exception ex)
        {
            Log?.LogWarning($"Could not patch the locale display name: {ex.GetType().Name}: {ex.Message}");
        }
    }

    private static void OverrideLocaleNativeName(Locale locale, ref string __result)
    {
        if (locale != null && locale.Identifier.Code == LocaleCode)
        {
            __result = LocaleDisplayName;
        }
    }
}
