using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerInfoPanel : MonoBehaviour
{
    [SerializeField] private GameObject sellInfo;
    
    [Header("References")]
    [SerializeField] private TMP_Text towerNameText;
    [SerializeField] private TMP_Text towerDamageText;
    [SerializeField] private TMP_Text towerRangeText;
    [SerializeField] private TMP_Text towerAttackSpeedText;
    [SerializeField] private TMP_Text towerSellText;
    
    public void Init(Tower tower, bool bought)
    {
        if (!bought) sellInfo.SetActive(false);
        else sellInfo.SetActive(true);
        
        towerNameText.text = tower.towerName;
        towerDamageText.text = tower.damage.ToString();

        if (tower.attackSpeed >= 3f) towerAttackSpeedText.text = new string("Very Fast");
        else if (tower.attackSpeed >= 2f) towerAttackSpeedText.text = new string("Fast");
        else if (tower.attackSpeed >= 1f) towerAttackSpeedText.text = new string("Normal");
        else if (tower.attackSpeed >= 0.5f) towerAttackSpeedText.text = new string("Slow");
        else towerAttackSpeedText.text = new string("Very Slow");

        towerRangeText.text = tower.range.ToString();
        towerSellText.text = (tower.cost * 0.7f).ToString();

    }
}
