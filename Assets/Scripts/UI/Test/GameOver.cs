using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public void OnClick_Restart(){
        MenuManager.OpenMenu(Menu.GameUI, gameObject);
    }

    public void OnClick_MainMenu(){
        MenuManager.OpenMenu(Menu.MainMenu, gameObject);
    }
}
