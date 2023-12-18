using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlaceTowerPrefab : MonoBehaviour {
    private SpriteRenderer[] spriteRenderers;
    public bool placeable;
    public bool waterUnit = false;

    //If we don't track all collisions with a list, the tower will become placeable on trigger exit even though there was a second collision.
    [SerializeField] private List<Collider2D> collisions = new List<Collider2D>();

    public GameObject towerPrefab;
    public Tower towerStats;

    public GameObject rangeIndicator;

    private void Awake() 
    {
        //Initialize the list spriteRenderers
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }
    
    private void Start() 
    {
        //Set the range indicator to the range of the tower
        rangeIndicator.transform.localScale = new Vector3(towerStats.range * 2, towerStats.range * 2, 0);

        //Towers always start out as unplaceable
        SetPlaceableFalse();
    }
    
    void Update() 
    {
        //PlaceTowerPrefab follows the mouse
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        //If the tower is a water unit and it exits a water tile, it is not placeable.
        if (waterUnit) 
        {
            if (other.CompareTag("water")) 
            {
                SetPlaceableFalse();

                //Remove the collision from the list of collisions.
                if (collisions.Contains(other)) collisions.Remove(other);
            }
            else if(other.CompareTag("hitbox"))
            {
                //Remove the collision from the list of collisions.
                if (collisions.Contains(other)) collisions.Remove(other);
                
                //If there are no more collisions with hitboxes, the tower is placeable.
                if (collisions.Count == 1) SetPlaceableTrue();
            }
        }
        //If the tower is not a water unit and it exits water or a hitbox, it will become placeable if the only collision is with the ground.
        else 
        {
            if (other.CompareTag("hitbox") || other.CompareTag("water")) 
            {
                //Remove the collision from the list of collisions.
                if (collisions.Contains(other)) collisions.Remove(other);
                
                //If there are no more collisions with hitboxes, the tower is placeable.
                if (collisions.Count == 1) SetPlaceableTrue();

            }
            else if (other.CompareTag("ground"))
            {
                //Remove the collision from the list of collisions.
                if (collisions.Contains(other)) collisions.Remove(other);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        //If the tower is a water unit and it enters a water tile, it is placeable if it is the only collision.
        if (waterUnit) 
        {
            if (other.CompareTag("water")) 
            {
                //If this is a new collision, add it to the list of collisions.
                if (!collisions.Contains(other)) collisions.Add(other);
                
                if (collisions.Count == 1) SetPlaceableTrue();
            }
            else if(other.CompareTag("hitbox"))
            {
                SetPlaceableFalse();
                
                //If this is a new collision, add it to the list of collisions.
                if (!collisions.Contains(other)) collisions.Add(other);
            }
        } 
        //If the tower is not a water unit and it enters a hitbox, it will become unplaceable.
        //If the tower enters a ground tile, it will become placeable if the only collision is with the ground.
        else 
        {
            if (other.CompareTag("hitbox") || other.CompareTag("water")) 
            {
                SetPlaceableFalse();

                //If this is a new collision, add it to the list of collisions.
                if (!collisions.Contains(other)) collisions.Add(other);
            }
            else if (other.CompareTag("ground"))
            {
                if (!collisions.Contains(other)) collisions.Add(other);
                
                if (collisions.Count == 1) SetPlaceableTrue();
            }
        }
    }
    
    
    private void SetPlaceableTrue()
    {
        //Visualize that the tower is placeable
        foreach (SpriteRenderer spriteRenderer in spriteRenderers) 
            spriteRenderer.color = new Color(Color.white.r, Color.white.g, Color.white.b, 0.5f);
        
        //The tower is placeable
        placeable = true;
    }
    
    private void SetPlaceableFalse()
    {
        //Visualize that the tower is not placeable
        foreach (SpriteRenderer spriteRenderer in spriteRenderers) 
            spriteRenderer.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.5f);
        
        //The tower is not placeable
        placeable = false;
    }
}