using System;
using EventBus;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public abstract class Tower : MonoBehaviour {
    [Header("Tower Stats")]
    [SerializeField] public float range;

    [SerializeField] public int damage;
    [SerializeField] public float attackSpeed;
    [SerializeField] protected float internalAttackSpeed;
    [SerializeField] public float bulletSpeed;
    public int cost = 1;
    [SerializeField] public string towerName = "Tower";

    [Header("Unity Setup Fields")]
    [SerializeField] public GameObject bulletPrefab;

    [SerializeField] public GameObject placePrefab;
    [SerializeField] public Transform firePoint;
    [SerializeField] public Transform target;
    [SerializeField] public CircleCollider2D rangeCollider;
    [SerializeField] public GameObject rangeIndicator;

    [SerializeField] public List<Enemy> enemiesInRange;
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    public bool selected;

    private bool buffed = false;


    protected virtual void OnEnable() {
        EventBus<MouseInputEvent>.Subscribe(CheckMouseOnTower);
    }

    protected virtual void OnDisable() {
        EventBus<MouseInputEvent>.Unsubscribe(CheckMouseOnTower);
    }

    protected virtual void Start() {
        //Set the range of the tower
        rangeCollider.radius = range;

        //Set the internal attack speed to the attack speed
        if (!buffed) internalAttackSpeed = 1 / attackSpeed;
        // Debug.Log("set attack speed");

        //Set the range indicator to the range of the tower
        rangeIndicator.transform.localScale = new Vector3(range * 2, range * 2, 0);
        rangeIndicator.SetActive(false);

        //Initialize the list spriteRenderers
        foreach (SpriteRenderer s in this.GetComponentsInChildren<SpriteRenderer>()) {
            spriteRenderers.Add(s);
        }
    }

    protected virtual IEnumerator FireCooldown(float wait) {
        //If there are enemies in range...
        if (enemiesInRange.Count > 0) {
            //...set the target to the first enemy in range,
            SetTarget(enemiesInRange);

            if (target != null) {
                Attack();
            }

            toRemoveAtEndOfFrame.ForEach(e => enemiesInRange.Remove(e));
            toRemoveAtEndOfFrame.Clear();

            //Wait for the fireRate before shooting again
            yield return new WaitForSeconds(wait);
            StartCoroutine(FireCooldown(internalAttackSpeed));
        } else yield return null;
    }

    #region Tower Methods

    public abstract void SetTarget(List<Enemy> enemies);

    public abstract void Attack();

    public void CheckMouseOnTower(MouseInputEvent e) {
        //If we receive a left mouse click and we are not selected...
        if (e.mouseButton == PlayerInput.MouseButton.Left && !selected) {
            //...check if the mouse is over one of the tower's sprites
            foreach (SpriteRenderer s in spriteRenderers) {
                if (s.bounds.Contains(e.mousePos)) {
                    //If the mouse is over one of the tower's sprites, select the tower
                    SelectTower(true);
                    break;
                }

                //If the mouse is not over one of the tower's sprites, deselect the tower.
                SelectTower(false);
            }
        }
        //If it was a right mouse click or it was already selected, deselect the tower.
        else SelectTower(false);
    }

    private void SelectTower(bool select) {
        //If this tower is selected...
        if (select) {
            //Remember that this tower is selected, show the range indicator and raise an Event to let other scripts know
            selected = true;
            rangeIndicator.SetActive(true);
            EventBus<TowerSelectedEvent>.Raise(new TowerSelectedEvent(this, true, true));
        }
        //If this tower is not selected...
        else {
            //Let other scripts know we are not selected
            rangeIndicator.SetActive(false);
            EventBus<TowerSelectedEvent>.Raise(new TowerSelectedEvent(this, true, false));

            //And remember that we are not selected
            selected = false;
        }
    }

    #endregion

    #region Enemies in Range

    //Keep track of enemies in range
    private void OnTriggerEnter2D(Collider2D collision) {
        //If an enemy enters the tower's range, add it to the list of enemies in range
        if (collision.CompareTag("Enemy")) {
            enemiesInRange.Add(collision.GetComponent<Enemy>());

            //If this is the first enemy to come into range, start shooting
            if (enemiesInRange.Count == 1) StartCoroutine(FireCooldown(internalAttackSpeed));
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        //If an enemy leaves the tower's range, remove it from the list of enemies in range
        if (collision.CompareTag("Enemy")) {
            // enemiesInRange.Remove(collision.GetComponent<Enemy>());

            toRemoveAtEndOfFrame.Add(collision.GetComponent<Enemy>());

            //If there are no more enemies in range, stop shooting
            if (enemiesInRange.Count == 0) StopAllCoroutines();
        }
    }

    private List<Enemy> toRemoveAtEndOfFrame = new();

    #endregion

    #region Getters and Setters

    public GameObject GetPlacePrefab() {
        return placePrefab;
    }


    public void SetInternalAttackSpeed(float modifier, bool alreadyBuffed = false) {
        if (!alreadyBuffed) {
            internalAttackSpeed = 1 / attackSpeed;
        }

        internalAttackSpeed *= modifier;

        buffed = true;
    }

    #endregion

    public void SetCost(int amount) {
        cost = amount;
    }
}