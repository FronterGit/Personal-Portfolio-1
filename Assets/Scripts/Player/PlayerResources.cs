using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEngine;

[System.Serializable]
public class PlayerResources : MonoBehaviour
{
    public static PlayerResources Instance { get; private set; }
    
    [SerializeField] private List<Resource> resources;
    
    //private Dictionary<string, Resource> resourceValues = new Dictionary<string, Resource>();
    
    public static Action<string, int> changeResourceAction;

    private void OnEnable()
    {
        EventBus<ChangeGoldEvent>.Subscribe(ChangeGold);
        EventBus<ChangeLivesEvent>.Subscribe(ChangeLives);
        
        changeResourceAction += ChangeResource;
        
        
    }

    private void OnDisable()
    {
        EventBus<ChangeGoldEvent>.Unsubscribe(ChangeGold);
        EventBus<ChangeLivesEvent>.Unsubscribe(ChangeLives);
        
        changeResourceAction -= ChangeResource;
    }

    private void Awake()
    {
        if(Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        InitializeResources();
        InitializeDictionary();
        
        Debug.Log("PlayerResources Instance set to " + Instance);
    }

    void Start()
    {
        foreach (var entry in Resource.resourceValues)
        {
            EventBus<UpdateUIEvent>.Raise(new UpdateUIEvent(entry.Key));
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
            Resource.resourceValues.Add(resource.name, resource);
        }
    }

    private void ChangeGold(ChangeGoldEvent e)
    {
        Resource.resourceValues["gold"].value += e.amount;
        EventBus<UpdateUIEvent>.Raise(new UpdateUIEvent("gold"));
    }
    
    private void ChangeLives(ChangeLivesEvent e)
    {
        Resource.resourceValues["lives"].value += e.amount;
        EventBus<UpdateUIEvent>.Raise(new UpdateUIEvent("lives"));
    }
    
    public void ChangeResource(string resource, int amount)
    {
        Resource.resourceValues[resource].value += amount;
        EventBus<UpdateUIEvent>.Raise(new UpdateUIEvent(resource));
    }

}
