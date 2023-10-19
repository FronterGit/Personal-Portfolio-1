using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlaceTowerPrefab : MonoBehaviour
{
    private SpriteRenderer[] spriteRenderers;
    public bool placeable = true;
    
    public GameObject towerPrefab;
    public Tower towerStats;

    public GameObject rangeIndicator;

    private void Start()
    {
        rangeIndicator.transform.localScale = new Vector3(towerStats.range * 2, towerStats.range * 2, 0);
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.5f);
        }

    }

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("hitbox"))
        {
            foreach(SpriteRenderer spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = new Color(Color.white.r, Color.white.g, Color.white.b, 0.5f);
                placeable = true;
            }
        }
        else if(other.CompareTag("ground"))
        {
            foreach(SpriteRenderer spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.5f);
                placeable = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("hitbox"))
        {
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.5f);
                placeable = false;
            }
        }
        else if (other.CompareTag("ground"))
        {
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = new Color(Color.white.r, Color.white.g, Color.white.b, 0.5f);
                placeable = true;
            }
        }
    }
}
