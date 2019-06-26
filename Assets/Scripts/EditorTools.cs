using System;
using System.Text;
using UnityEditor;
using UnityEngine;

public class EditorTools : MonoBehaviour
{   
    [MenuItem("Assets/Tools")]
    [MenuItem("Assets/Tools/GetSelectionResName", priority = 0)]
    static void GetSelectionResName()
    {
        StringBuilder sb = new StringBuilder();
        var res = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.TopLevel);
        foreach (var item in res)
        {
            sb.AppendLine(item.name);
        }
        Debug.LogError(sb.ToString());
    }

    [MenuItem("Assets/Tools/GetAssetsPath")]
    static void GetAssetsPath()
    {
        var res = Selection.GetFiltered<UnityEngine.Object>(SelectionMode.Assets);
        foreach (var item in res)
        {
            Debug.LogError(AssetDatabase.GetAssetPath(item));
        }
    }
}
