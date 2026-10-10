using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using BepInEx.Logging;

namespace DressmakerLATAM;

internal static class RuntimeCsv
{
    public static Dictionary<long, string> Load(string path, ManualLogSource? log)
    {
        string text = File.ReadAllText(path, Encoding.UTF8);
        List<List<string>> rows = Parse(text);
        Dictionary<long, string> result = new();

        for (int i = 1; i < rows.Count; i++)
        {
            List<string> row = rows[i];
            if (row.Count < 2)
            {
                log?.LogWarning($"{Path.GetFileName(path)} row {i + 1}: expected at least 2 columns; ignored.");
                continue;
            }

            string idText = row[0].Trim();
            string value = row[row.Count - 1];
            if (!long.TryParse(idText, NumberStyles.Integer, CultureInfo.InvariantCulture, out long id))
            {
                log?.LogWarning($"{Path.GetFileName(path)} row {i + 1}: invalid ID '{idText}'; ignored.");
                continue;
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                log?.LogWarning($"{Path.GetFileName(path)} row {i + 1}: empty translation for ID {id}; ignored.");
                continue;
            }

            if (result.ContainsKey(id))
            {
                log?.LogWarning($"{Path.GetFileName(path)} row {i + 1}: duplicate ID {id}; last value wins.");
            }

            result[id] = value.Replace("\r\n", "\n");
        }

        return result;
    }

    private static List<List<string>> Parse(string text)
    {
        List<List<string>> rows = new();
        List<string> row = new();
        StringBuilder field = new();
        bool quoted = false;

        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];

            if (quoted)
            {
                if (c == '"')
                {
                    if (i + 1 < text.Length && text[i + 1] == '"')
                    {
                        field.Append('"');
                        i++;
                    }
                    else
                    {
                        quoted = false;
                    }
                }
                else
                {
                    field.Append(c);
                }

                continue;
            }

            if (c == '"')
            {
                quoted = true;
            }
            else if (c == ',')
            {
                row.Add(field.ToString());
                field.Clear();
            }
            else if (c == '\r' || c == '\n')
            {
                if (c == '\r' && i + 1 < text.Length && text[i + 1] == '\n')
                {
                    i++;
                }

                row.Add(field.ToString());
                field.Clear();
                rows.Add(row);
                row = new List<string>();
            }
            else
            {
                field.Append(c);
            }
        }

        if (field.Length > 0 || row.Count > 0)
        {
            row.Add(field.ToString());
            rows.Add(row);
        }

        return rows;
    }
}
