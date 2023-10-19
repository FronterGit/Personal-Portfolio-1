using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[System.Serializable]
public class TowerCost
{
    public Tower tower;
    public int cost;
    public TMP_Text costText;
    public TMP_Text towerNameText;
}
public class Shop : MonoBehaviour
{
    private bool buying;
    private GameObject towerToBuy;
    private Tower towerScript;
    [SerializeField] private List<TowerCost> towersCosts;

    

    private void OnEnable()
    {
        EventBus<MouseInputEvent>.Subscribe(OnTowerBuy);
    }

    public void BuyTower(Tower tower)
    {
        if(buying) return;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        towerToBuy = Instantiate(tower.GetPlacePrefab(), mousePos, Quaternion.identity);
        towerScript = towerToBuy.GetComponent<Tower>();
        buying = true;
    }

    private void Start()
    {
        InitializeShop();
    }

    private void InitializeShop()
    {
        foreach (TowerCost towerCost  in towersCosts)
        {
            towerCost.tower.SetCost(towerCost.cost);
            towerCost.costText.text = towerCost.cost.ToString();
            towerCost.towerNameText.text = towerCost.tower.name;
        }
        
    }

    private void OnTowerBuy(MouseInputEvent e)
    {
        if(!buying) return;
        if (e.mouseButton == PlayerInput.MouseButton.Left)
        {
            if(Resource.resourceValues["gold"]?.value > towerScript.cost)
            {
                //PlayerResources.Instance.ChangeGold(-towerScript.cost);
                Destroy(towerToBuy);
                buying = false;
            }

        }
        else
        {
            Destroy(towerToBuy);
            buying = false;
        }
    }

}
