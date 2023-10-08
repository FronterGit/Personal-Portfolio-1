using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private bool buying;
    private GameObject towerToBuy;
    public void BuyTower(Tower tower)
    {
        if(buying) return;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        towerToBuy = Instantiate(tower.GetPlacePrefab(), mousePos, Quaternion.identity);
        buying = true;
    }

    public void Update()
    {
        Buying();
    }

    public void Buying()
    {
        if(!buying) return;
    }
}
