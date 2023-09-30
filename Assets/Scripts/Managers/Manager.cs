using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static List<Manager> instances = new List<Manager>();
    private void Awake()
    {
        if(!instances.Contains(this))
        {
            instances.Add(this);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
