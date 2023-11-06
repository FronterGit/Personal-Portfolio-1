using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerInfoPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text towerNameText;
    [SerializeField] private GameObject sellInfo;
    public void Init(Tower tower, bool bought)
    {
        if (!bought) sellInfo.SetActive(false);
        else sellInfo.SetActive(true);
        
        towerNameText.text = tower.towerName;
    }
}
