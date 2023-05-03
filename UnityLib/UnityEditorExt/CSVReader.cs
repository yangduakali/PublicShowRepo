using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
// ReSharper disable InconsistentNaming

namespace UnityEditorExt;

public class CSVReader
{
    private const string SPLIT_RE = "[;,](?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))";
    private const string LINE_SPLIT_RE = "\\r\\n|\\n\\r|\\n|\\r";
    private static readonly char[] TRIM_CHARS = { '"' };

    public static List<Dictionary<string, object>> Read(string filePath)
    {
        List<Dictionary<string, object>> dictionaryList = new List<Dictionary<string, object>>();
        string[] strArray1 = Regex.Split(AssetDatabase.LoadAssetAtPath<TextAsset>(filePath).text, LINE_SPLIT_RE);
        if (strArray1.Length <= 1)
            return dictionaryList;
        string[] strArray2 = Regex.Split(strArray1[0], SPLIT_RE);
        for (int index1 = 1; index1 < strArray1.Length; ++index1)
        {
            string[] strArray3 = Regex.Split(strArray1[index1], SPLIT_RE);
            if (strArray3.Length == 0 || strArray3[0] == "") continue;
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            for (int index2 = 0; index2 < strArray2.Length && index2 < strArray3.Length; ++index2)
            {
                string s = strArray3[index2].TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object obj = s;
                if (int.TryParse(s, out var result1))
                {
                    obj = result1;
                }
                else
                {
                    if (float.TryParse(s, out var result2))
                        obj = result2;
                }
                dictionary[strArray2[index2]] = obj;
            }
            dictionaryList.Add(dictionary);
        }
        return dictionaryList;
    }
}