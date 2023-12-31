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
    public TMP_Text towerNameText;
    public TMP_Text costText;
}
public class Shop : MonoBehaviour
{
    [Header("Variables")]
    private bool buying;
    private GameObject towerHover;
    private PlaceTowerPrefab towerToBuy;
    private List<Tower> selectedTowers = new List<Tower>();
    
    [Header("References")]
    [SerializeField] private List<TowerCost> towersCosts;
    [SerializeField] private GameObject TowerInfoPanel;

    [Header("Prompts")] 
    [SerializeField] private TMP_Text buyingPrompt;
    [SerializeField] private TMP_Text notEnoughGoldPrompt;
    
    private void OnEnable()
    {
        //Subscribe to the TowerSelectedEvent so we can select towers.
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
            towerCost.costText.text = towerCost.tower.cost.ToString();
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
        towerToBuy = towerHover.GetComponent<PlaceTowerPrefab>();
        towerToBuy.towerStats = tower;
        
        //Open the towerSelectedPanel and display the tower's stats.
        TowerInfoPanel.SetActive(true);
        TowerInfoPanel.GetComponent<TowerInfoPanel>().Init(tower, false);
        
        //Go in to buying mode.
        Buying(true);
        
        //Raise display event to tell canvas to display prompt
        EventBus<DisplayPromptEvent>.Raise(new DisplayPromptEvent(buyingPrompt));
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
                
                //Raise display event to tell canvas to stop displaying prompt
                EventBus<DisplayPromptEvent>.Raise(new DisplayPromptEvent(null, false));
            }
            else if(!towerToBuy.placeable)
            {
                //do nothing
            }
            else
            {
                Destroy(towerHover);
                Buying(false);
                
                //Raise display event to tell canvas to display not enough gold prompt
                EventBus<DisplayPromptEvent>.Raise(new DisplayPromptEvent(notEnoughGoldPrompt, true, 2f));
            }
        }
        else
        {
            Destroy(towerHover);
            Buying(false);
            
            //Raise display event to tell canvas to stop displaying prompt
            EventBus<DisplayPromptEvent>.Raise(new DisplayPromptEvent(null, false));
        }
    }
    
    private void Buying(bool setTo = true)
    {
        if (setTo)
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

    public void SellTower()
    {
        //Grant the player 70% of the tower's cost and destroy the tower.
        ResourceManager.changeResourceAction?.Invoke("gold", Mathf.RoundToInt(selectedTowers[0].GetComponent<Tower>().cost * 0.7f));
        Destroy(selectedTowers[0].gameObject);
        
        //Deselect the tower and close the towerSelectedPanel.
        selectedTowers.Clear();
        TowerInfoPanel.SetActive(false);
    }

    private void PlaceTower()
    {
        //If the tower has successfully been bought, instantiate the tower and destroy the placeTowerPrefab.
        Instantiate(towerToBuy.towerPrefab, towerHover.transform.position, Quaternion.identity);
        ResourceManager.changeResourceAction?.Invoke("gold", -towerToBuy.towerStats.cost);
        Destroy(towerHover);
        
        //Then close the towerSelectedPanel.
        TowerInfoPanel.SetActive(false);

        EventBus<TowerPlacedEvent>.Raise(new TowerPlacedEvent());
    }
    
    private void TowerSelection(TowerSelectedEvent e)
    {
        //If the event wants us to select a tower, clear the list and then add it to the selectedTowers list.
        if (e.selected)
        {
            selectedTowers.Clear();
            selectedTowers.Add(e.tower);
        }
        //Else, remove it from the list. If we don't do this, the info panel will stay open even though we have deselected the tower.
        else selectedTowers.Remove(e.tower);

        //If we have exactly one tower selected, open the towerSelectedPanel and display the tower's stats.
        if (selectedTowers.Count == 1)
        {
            TowerInfoPanel.SetActive(true);
            if (e.bought)
            { 
                TowerInfoPanel.GetComponent<TowerInfoPanel>().Init(selectedTowers[0], true);
            }
            else TowerInfoPanel.GetComponent<TowerInfoPanel>().Init(selectedTowers[0], false);
        }
        else if(selectedTowers.Count > 1) Debug.LogError("More than one tower selected!");
        //If we don't have any towers selected, close the towerSelectedPanel.
        else TowerInfoPanel.SetActive(false);


    }
    

}
