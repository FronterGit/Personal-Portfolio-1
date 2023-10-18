using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEngine;

[System.Serializable]
public class PlayerResources : MonoBehaviour
{
    public static PlayerResources Instance { get; private set; }
    [field: SerializeField] public int gold { get; private set;}
    [field: SerializeField] public int lives { get; private set;}

    private void OnEnable()
    {
        EventBus<ChangeGoldEvent>.Subscribe(ChangeGold);
        EventBus<ChangeLivesEvent>.Subscribe(ChangeLives);
    }

    private void OnDisable()
    {
        EventBus<ChangeGoldEvent>.Unsubscribe(ChangeGold);
        EventBus<ChangeLivesEvent>.Unsubscribe(ChangeLives);
    }

    private void Awake()
    {
        if(Instance != null && Instance != this) Destroy(this);
        else Instance = this;
        
        Debug.Log("PlayerResources Instance set to " + Instance);
    }

    void Start()
    {
        EventBus<UpdateUIEvent>.Raise(new UpdateUIEvent("gold", gold));
        EventBus<UpdateUIEvent>.Raise(new UpdateUIEvent("lives", lives));
    }

    private void ChangeGold(ChangeGoldEvent e)
    {
        gold += e.amount;
    }
    
    private void ChangeLives(ChangeLivesEvent e)
    {
        lives += e.amount;
    }

    private void Update()
    {
    }
    
}
