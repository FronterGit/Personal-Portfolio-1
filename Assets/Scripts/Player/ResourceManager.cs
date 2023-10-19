using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class ResourceManager : MonoBehaviour
{
    private Guid guid = Guid.NewGuid();
    
    [SerializeField] private List<Resource> resources;
    public Dictionary<string, Resource> resourceValues = new();
    
    public static Action<string, int> changeResourceAction;
    public static Func<string, int> getResourceValueFunc;

    private void OnEnable()
    {
        changeResourceAction += ChangeResource;
        getResourceValueFunc += GetResourceValue;
    }

    private void OnDisable()
    {
        changeResourceAction -= ChangeResource;
        getResourceValueFunc -= GetResourceValue;
        
    }

    private void Awake()
    {
        InitializeResources();
        InitializeDictionary();
    }

    void Start()
    {
        foreach (var entry in resourceValues)
        {
            UpdateResourceUI(resourceValues[entry.Key].name, resourceValues[entry.Key].value);
        }
    }

    private void InitializeResources()
    {
        foreach (Resource resource in resources)
        {
            Resource.resources.Add(resource);
        }
    }
    
    private void InitializeDictionary()
    {
        foreach (Resource resource in Resource.resources)
        {
            resourceValues.Add(resource.name, resource);
        }
        Resource.resourcesContainer.Add(guid, resourceValues);
    }
    
    public void ChangeResource(string resource, int amount)
    {
        int newValue = Resource.resourcesContainer[guid][resource].value += amount;    
        UpdateResourceUI(resource, newValue);
    }

    private void UpdateResourceUI(string resource, int amount)
    {
        Resource.resourcesContainer[guid][resource].text.text = amount.ToString();    
    }
    
    public int GetResourceValue(string resource)
    {
        return Resource.resourcesContainer[guid][resource].value;
    }

}
