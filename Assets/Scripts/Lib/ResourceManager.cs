using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResourceManager : Singleten<ResourceManager>
{
    Dictionary<string, Object> cacheResourceMap = new Dictionary<string, Object>();

    public T Load<T>(string path) where T : Object
    {
        if (!cacheResourceMap.ContainsKey(path))
        {
            T resource = AssetDatabase.LoadAssetAtPath<T>(path);
            cacheResourceMap.Add(path, resource);
        }
        return Instantiate(cacheResourceMap[path] as T);
    }

}
