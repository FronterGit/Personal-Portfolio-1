using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class ResourceManager : MonoBehaviour
{
    //The Resource Manager has a unique guid, so that it is the only script that is allowed to change resources.
    private Guid guid = Guid.NewGuid();
    
    [SerializeField] private List<Resource> resources;
    public Dictionary<string, Resource> resourceValues = new();
    
    public static Action<string, int> changeResourceAction;
    public static Action resetResourcesAction;
    public static Func<string, int> getResourceValueFunc;

    private void OnEnable()
    {
        changeResourceAction += ChangeResource;
        getResourceValueFunc += GetResourceValue;
        resetResourcesAction += ResetResources;
    }

    private void OnDisable()
    {
        changeResourceAction -= ChangeResource;
        getResourceValueFunc -= GetResourceValue;
        resetResourcesAction -= ResetResources;
        
    }

    private void Awake()
    {
        InitializeResources();
        InitializeDictionary();
    }

    void Start()
    {
        //Update the UI with the initial values
        foreach (var entry in resourceValues)
        {
            UpdateResourceUI(resourceValues[entry.Key].name, resourceValues[entry.Key].value);
        }
    }

    private void InitializeResources()
    {
        //Add all resources to a static list
        foreach (Resource resource in resources)
        {
            Resource.resources.Add(resource);
        }
    }
    
    private void InitializeDictionary()
    {
        //Add all resources to a static dictionary
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
        //Use our guid to access the dictionary and change the UI
        Resource.resourcesContainer[guid][resource].text.text = amount.ToString();    
    }
    
    public int GetResourceValue(string resource)
    {
        //Use our guid to access the dictionary and return the value
        return Resource.resourcesContainer[guid][resource].value;
    }
    
    private void ResetResources()
    {
        //Because we are using a static list, we need to clear it when we load a new scene
        Resource.resources.Clear();
        Resource.resourcesContainer.Clear();
    }

}
