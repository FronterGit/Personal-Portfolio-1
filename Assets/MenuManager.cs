using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private Menu activeMenu;
    [SerializeField] private Menu mainMenu;

    private void Start()
    {
        //Close all menus
        GetComponentInChildren<IMenu>().CloseMenu();
        
        //Open the main menu
        mainMenu.OpenMenu();
        activeMenu = mainMenu;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public void OpenMenu(Menu menu)
    {
        activeMenu.CloseMenu();
        menu.OpenMenu();
        activeMenu = menu;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
