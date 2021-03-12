using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedSingleTon : MonoBehaviour
{
    public static SeedSingleTon instance = null;
    public int seed;
    
    public static SeedSingleTon Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        Debug.LogWarning(seed);
    }
}
