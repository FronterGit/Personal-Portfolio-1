using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shop : MonoBehaviour
{
    private bool buying;
    private GameObject towerToBuy;

    private void OnEnable()
    {
        EventBus<MouseInputEvent>.Subscribe(OnTowerBuy);
    }

    public void BuyTower(Tower tower)
    {
        if(buying) return;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        towerToBuy = Instantiate(tower.GetPlacePrefab(), mousePos, Quaternion.identity);
        buying = true;
    }

    public void Update()
    {
    }

    private void OnTowerBuy(MouseInputEvent e)
    {
        if(!buying) return;
        if (e.mouseButton == PlayerInput.MouseButton.Left)
        {
            if(PlayerResources.Instance.gold > 5) Debug.Log("tower can be bought");

        }
        else
        {
            Destroy(towerToBuy);
            buying = false;
        }
    }

}
