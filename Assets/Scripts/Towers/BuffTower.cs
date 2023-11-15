using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEngine;

public class BuffTower : Tower {
    [SerializeField] private float attackSpeedModifier = 1.2f;

    [SerializeField] private List<Tower> buffedTowers = new();

    protected override void Start() {
        base.Start();

        Attack();
    }

    protected override void OnEnable() {
        base.OnEnable();

        EventBus<TowerPlacedEvent>.Subscribe(onTowerPlaced);
    }

    protected override void OnDisable() {
        foreach (var kvp in buffedTowers) {
            kvp.SetInternalAttackSpeed(1f / 1.2f, true);
        }

        EventBus<TowerPlacedEvent>.Unsubscribe(onTowerPlaced);


        base.OnDisable();
    }


    public override void SetTarget(List<Enemy> enemies) {
        throw new System.NotImplementedException();
    }

    public override void Attack() {
        var list = Physics2D.CircleCastAll(transform.position, range, Vector2.zero);
        Debug.Log(list.Length);
        foreach (var hit in list) {
            var l = hit.collider.GetComponent<Tower>();
            if (l == null) {
                continue;
            }

            if (l == this) {
                continue;
            }

            if (buffedTowers.Contains(l)) {
                Debug.Log("ALREADY FKN EXIST");
                continue;
            }

            l.SetInternalAttackSpeed(attackSpeedModifier);
            buffedTowers.Add(l);

            Debug.Log("added tower");
        }
    }

    private void onTowerPlaced(TowerPlacedEvent e) {
        Attack();
    }

    private IEnumerator next(Action action) {
        yield return new WaitForEndOfFrame();
        action.Invoke();
    }
}