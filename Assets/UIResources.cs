using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Resource
{
    public string kvp;
    public TMP_Text text;
}
public class UIResources : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private List<Resource> resources;
    private Dictionary<string, TMP_Text> resourceTexts = new Dictionary<string, TMP_Text>();

    private void OnEnable()
    {
        EventBus<UpdateUIEvent>.Subscribe(UpdateUI);
    }

    private void Start()
    {
        InitializeDictionary();
    }
    
    private void InitializeDictionary()
    {
        foreach (Resource resource in resources)
        {
            resourceTexts.Add(resource.kvp, resource.text);
        }
    }

    private void UpdateUI(UpdateUIEvent e)
    {
        resourceTexts[e.kvp].text = e.value.ToString();
    }
}
