using System.Collections.Generic;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DressmakerLATAM;

internal sealed class LatamTableProvider : ITableProvider
{
    private readonly ITableProvider previousProvider;
    private readonly Dictionary<string, Dictionary<long, string>> tables;
    private readonly ManualLogSource? log;

    public LatamTableProvider(
        ITableProvider previousProvider,
        Dictionary<string, Dictionary<long, string>> tables,
        ManualLogSource? log)
    {
        this.previousProvider = previousProvider;
        this.tables = tables;
        this.log = log;
    }

    public AsyncOperationHandle<TTable> ProvideTableAsync<TTable>(string tableReference, Locale locale)
        where TTable : LocalizationTable
    {
        if (locale == null || locale.Identifier.Code != Plugin.LocaleCode)
        {
            return previousProvider.ProvideTableAsync<TTable>(tableReference, locale);
        }

        string englishAddress = tableReference + "_en";
        AsyncOperationHandle<TTable> englishHandle = Addressables.LoadAssetAsync<TTable>(englishAddress);
        return Addressables.ResourceManager.CreateChainOperation(englishHandle, handle =>
        {
            TTable cloned = CloneAndApply(handle.Result, tableReference);
            return Addressables.ResourceManager.CreateCompletedOperation(cloned, null);
        });
    }

    private TTable CloneAndApply<TTable>(TTable source, string tableReference)
        where TTable : LocalizationTable
    {
        TTable clone = Object.Instantiate(source);
        clone.name = source.name.Replace("_en", "_es-419");
        clone.LocaleIdentifier = new LocaleIdentifier(Plugin.LocaleCode);

        int applied = 0;
        int missing = 0;
        if (clone is StringTable stringTable && tables.TryGetValue(tableReference, out Dictionary<long, string> translations))
        {
            HashSet<long> found = new();

            foreach (StringTableEntry entry in stringTable.Values)
            {
                long id = entry.KeyId;
                if (translations.TryGetValue(id, out string value))
                {
                    entry.Value = value;
                    found.Add(id);
                    applied++;
                }
                else
                {
                    missing++;
                }
            }

            foreach (long id in translations.Keys)
            {
                if (!found.Contains(id))
                {
                    log?.LogWarning($"{tableReference}: CSV ID not found in table: {id}");
                }
            }

            log?.LogInfo($"{tableReference}: cloned for {Plugin.LocaleCode}; applied {applied}, missing {missing}, csv {translations.Count}.");
        }
        else if (clone is AssetTable)
        {
            log?.LogInfo($"{tableReference}: asset table cloned for {Plugin.LocaleCode} using English asset references.");
        }
        else
        {
            log?.LogWarning($"{tableReference}: no CSV translations loaded or table is not a StringTable; cloned without changes.");
        }

        return clone;
    }
}
