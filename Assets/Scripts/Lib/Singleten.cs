using UnityEngine;


/// <summary>
/// 继承 MonoBehaviour 的单例父类
/// </summary>
/// <typeparam name="T">单例类</typeparam>
public class Singleten<T> : MonoBehaviour where T: MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this as T;
    }
}
