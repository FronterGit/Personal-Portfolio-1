using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class UIResources : MonoBehaviour
{
    private void OnEnable()
    {
        EventBus<UpdateUIEvent>.Subscribe(UpdateUI);
    }

    private void OnDisable()
    {
        EventBus<UpdateUIEvent>.Unsubscribe(UpdateUI);
    }


    private void UpdateUI(UpdateUIEvent e)
    {
        Resource.resourceValues[e.name].text.text = Resource.resourceValues[e.name].value.ToString();
    }
}
