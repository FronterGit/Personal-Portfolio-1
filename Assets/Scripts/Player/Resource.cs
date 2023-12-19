using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Resource
{
    public string name;
    public int value;
    public TMP_Text text;

    public static List<Resource> resources = new();
    public static Overseer<System.Guid, Dictionary<string, Resource>> resourcesContainer = new();
    
    public void Reset()
    {
        resources.Clear();
        resourcesContainer.Clear();
    }
}

