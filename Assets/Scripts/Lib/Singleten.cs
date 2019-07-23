using UnityEngine;

/// <summary>
/// 继承 MonoBehaviour 的单例父类
/// </summary>
/// <typeparam name="T">单例类</typeparam>
public class SingletenMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject(typeof(T).Name).AddComponent<T>();
                DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }

    public void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<T>();
            DontDestroyOnLoad(instance);
        }
    }
}

/// <summary>
/// 单例父类
/// </summary>
/// <typeparam name="T">单例类</typeparam>
public class Singleten<T> where T : new()
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }
}