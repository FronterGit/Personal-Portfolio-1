using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour, IMenu
{
    public void OpenMenu()
    {
        gameObject.SetActive(true);
    }
    
    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }
}
