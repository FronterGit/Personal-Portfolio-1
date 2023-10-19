using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Resource
{
    public string name;
    public int value;
    public TMP_Text text;

    public static List<Resource> resources = new ();
    public static Dictionary<string, Resource> resourceValues = new();
}
