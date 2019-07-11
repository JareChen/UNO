using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 配置管理类
public class ConfigManager : Singleten<ConfigManager>
{
    /// <summary>
    /// 缓存配置，这里类型转换要用 IDictionary, 这个才是父类，用 IDictionary<> 或者 Dictionary<> 会重新生成对应的类型，无法类型转换。
    /// </summary>
    Dictionary<string, IDictionary> configListMap = new Dictionary<string, IDictionary>();

    /// <summary>
    /// 读取整个配置表
    /// </summary>
    /// <typeparam name="T">配置类</typeparam>
    /// <returns>数据字典</returns>
    public Dictionary<string, T> LoadConfigs<T>() where T : Config, new()
    {
        var configName = typeof(T).Name;
        if (!configListMap.ContainsKey(configName))
        {
            Dictionary<string, T> configList = new Dictionary<string, T>();
            TextAsset ta = ResourceManager.Instance.Load<TextAsset>(configName);
            var strLines = ta.text.Split('\n');
            foreach (var item in strLines)
            {
                T t = new T();
                t.ParseDataRow(item);
                configList.Add(t.Id, t);
            }
            configListMap.Add(configName, configList as IDictionary);
        }
        return configListMap[configName] as Dictionary<string, T>;
    }

    /// <summary>
    /// 读取单个配置
    /// </summary>
    /// <typeparam name="T">配置类</typeparam>
    /// <param name="id">单个数据</param>
    /// <returns></returns>
    public T LoadConfigById<T>(string id) where T : Config, new()
    {
        var configName = typeof(T).Name;
        var configs = LoadConfigs<T>();
        if (configs.ContainsKey(id))
        {
            return configListMap[configName][id] as T;
        }
        Debug.LogError(string.Format("获取配置失败，类型: {0}，ID: {1}", configName, id));
        return null;
    }
}
