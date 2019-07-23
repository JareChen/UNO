using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Messager : Singleten<Messager>
{
    Dictionary<string, List<Action>> msgPool = new Dictionary<string, List<Action>>();

    public void AddListener(string key, Action action)
    {
        if (!msgPool.ContainsKey(key))
        {
            msgPool[key] = new List<Action>();
        }
        msgPool[key].Add(action);
    }

    public void RemoveListener(string key, Action action)
    {
        if (msgPool.ContainsKey(key))
        {
            msgPool[key].Remove(action);
        }
    }

    public void Broadcast(string key)
    {
        if (msgPool.ContainsKey(key))
        {
            foreach (var item in msgPool[key])
            {
                item?.Invoke();
            }
        }
    }

    public void Clear()
    {
        msgPool.Clear();
    }
}
