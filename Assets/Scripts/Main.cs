using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private void Awake()
    {
        gameObject.AddComponent<ResourceManager>();
        gameObject.AddComponent<ConfigManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        var config = ConfigManager.Instance.LoadConfigById<CardConfig>("1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
