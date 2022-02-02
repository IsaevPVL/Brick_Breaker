using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MenuManager
{   
    public static bool isInitialised { get; private set; }
    public static GameObject mainMenu, gameUI, gameOver;

    public static void Initialise(){
        GameObject canvas = GameObject.Find("Canvas");
        mainMenu = canvas.transform.Find("Main Menu").gameObject;
        gameUI = canvas.transform.Find("Game UI").gameObject;
        gameOver = canvas.transform.Find("Game Over").gameObject;

        isInitialised = true;
    }

    public static void OpenMenu(Menu menu, GameObject callingMenu){
        if(!isInitialised){
            Initialise();
        }
        switch(menu){
            case Menu.MainMenu:
                mainMenu.SetActive(true);
                break;
            case Menu.GameUI:
                gameUI.SetActive(true);
                break;
            case Menu.GameOver:
                gameOver.SetActive(true);
                break;

        }
        callingMenu.SetActive(false);

    }
}