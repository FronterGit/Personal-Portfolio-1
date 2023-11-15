using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlaceTowerPrefab : MonoBehaviour {
    private SpriteRenderer[] spriteRenderers;
    public bool placeable;

    //If we don't track all collisions with a list, the tower will become placeable on trigger exit even though there was a second collision.
    [SerializeField] private List<Collider2D> collisions = new List<Collider2D>();

    public GameObject towerPrefab;
    public Tower towerStats;

    public GameObject rangeIndicator;

    private void Start() {
        rangeIndicator.transform.localScale = new Vector3(towerStats.range * 2, towerStats.range * 2, 0);

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spriteRenderer in spriteRenderers) {
            spriteRenderer.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.5f);
        }
    }

    void Update() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("hitbox")) {
            //Remove the collision from the list of collisions.
            if (collisions.Contains(other)) collisions.Remove(other);

            //If there are no more collisions with hitboxes, the tower is placeable.
            if (collisions.Count == 0) {
                foreach (SpriteRenderer spriteRenderer in spriteRenderers) {
                    spriteRenderer.color = new Color(Color.white.r, Color.white.g, Color.white.b, 0.5f);
                    placeable = true;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("hitbox")) {
            foreach (SpriteRenderer spriteRenderer in spriteRenderers) {
                spriteRenderer.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.5f);
            }

            placeable = false;

            //If this is a new collision, add it to the list of collisions.
            if (!collisions.Contains(other)) collisions.Add(other);
        }
    }
}