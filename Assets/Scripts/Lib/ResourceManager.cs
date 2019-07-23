using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResourceManager : SingletenMono<ResourceManager>
{
    Dictionary<string, Object> cacheResourceMap = new Dictionary<string, Object>();

    public T Load<T>(string path) where T : Object
    {
        var type = typeof(T);
        if (type == typeof(Sprite))
        {
            path = Const.artPath + path + Const.spriteAssetExt;
        }
        else if (type == typeof(TextAsset))
        {
            path = Const.configPath + path + Const.textAssetExt;
        }
        else if(type == typeof(Transform) || type == typeof(GameObject))
        {
            path = Const.prefabPath + path + Const.prefabAssetExt;
        }

        if (!cacheResourceMap.ContainsKey(path))
        {
            T resource = AssetDatabase.LoadAssetAtPath<T>(path);
            cacheResourceMap.Add(path, resource);
        }
        return Instantiate(cacheResourceMap[path] as T);
    }

}
