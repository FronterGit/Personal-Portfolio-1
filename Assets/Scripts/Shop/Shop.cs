using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
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
    private GameObject towerHover;
    private PlaceTowerPrefab towerToBuy;
    private List<Tower> selectedTowers = new List<Tower>();
    
    [SerializeField] private List<TowerCost> towersCosts;
    [SerializeField] private GameObject TowerInfoPanel;
    
    private void OnEnable()
    {
        EventBus<TowerSelectedEvent>.Subscribe(TowerSelection);
    }

    private void OnDisable()
    {
        EventBus<TowerSelectedEvent>.Unsubscribe(TowerSelection);
        EventBus<MouseInputEvent>.Unsubscribe(OnTowerBuy);
    }

    private void Start()
    {
        InitializeShop();
    }

    private void InitializeShop()
    {
        //Set the cost of each tower and display it in the shop UI.
        foreach (TowerCost towerCost  in towersCosts)
        {
            towerCost.tower.SetCost(towerCost.cost);
            towerCost.costText.text = towerCost.cost.ToString();
            towerCost.towerNameText.text = towerCost.tower.name;
        }
    }
    
    public void BuyTower(Tower tower)
    {
        //If the player is already buying a tower, return.
        if(buying) return;
        
        //Instantiate the placeTowerPrefab. This is a copy of the tower prefab that is used to show the player
        //where the tower will be placed.
        var placePrefab = tower.GetPlacePrefab();
        towerHover = Instantiate(placePrefab, Vector3.zero, Quaternion.identity);
        
        //Set the towerToBuy and the towerStats so we can use them later.
        towerToBuy = placePrefab.GetComponent<PlaceTowerPrefab>();
        towerToBuy.towerStats = tower;
        
        //Open the towerSelectedPanel and display the tower's stats.
        TowerInfoPanel.SetActive(true);
        TowerInfoPanel.GetComponent<TowerInfoPanel>().Init(tower, false);
        
        Buying(true);
    }

    private void OnTowerBuy(MouseInputEvent e)
    {
        //If the player is not buying a tower or the towerHover is null, return.
        if(!buying || towerHover == null) return;
        
        //If the player clicks the left mouse button, try to place the tower.
        if (e.mouseButton == PlayerInput.MouseButton.Left)
        {
            if(ResourceManager.getResourceValueFunc("gold") >= towerToBuy.towerStats.cost && towerToBuy.placeable)
            {
                PlaceTower();
                Buying(false);
            }
        }
        else
        {
            Destroy(towerHover);
            Buying(false);
        }
    }
    
    private void Buying(bool set = true)
    {
        if (set)
        {
            //Set the buying bool to true so we know we are currently trying to buy a tower and want to listen for mouse input.
            buying = true;
            EventBus<MouseInputEvent>.Subscribe(OnTowerBuy);
        }
        else
        {
            //We are no longer buying a tower, so set buying to false and unsubscribe from the mouse input event.
            buying = false;
            EventBus<MouseInputEvent>.Unsubscribe(OnTowerBuy);
            
            //We no longer want to display the tower's stats, so close the towerSelectedPanel.
            TowerInfoPanel.SetActive(false);
        }
    }

    private void PlaceTower()
    {
        //If the tower has successfully been bought, instantiate the tower and destroy the placeTowerPrefab.
        Instantiate(towerToBuy.towerPrefab, towerHover.transform.position, Quaternion.identity);
        ResourceManager.changeResourceAction?.Invoke("gold", -towerToBuy.towerStats.cost);
        Destroy(towerHover);
        
        //Then close the towerSelectedPanel.
        TowerInfoPanel.SetActive(false);
    }
    
    private void TowerSelection(TowerSelectedEvent e)
    {
        if(e.selected) selectedTowers.Add(e.tower);
        else selectedTowers.Remove(e.tower);

        if (selectedTowers.Count > 0)
        {
            foreach (Tower t in selectedTowers)
            {
                TowerInfoPanel.SetActive(true);

                if (e.bought)
                {
                    TowerInfoPanel.GetComponent<TowerInfoPanel>().Init(t, true);
                }
                else TowerInfoPanel.GetComponent<TowerInfoPanel>().Init(t, false);
            }
        }
        else TowerInfoPanel.SetActive(false);


    }
    

}
