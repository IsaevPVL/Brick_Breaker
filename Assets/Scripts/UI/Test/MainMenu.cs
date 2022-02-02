using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void OnClick_Start(){
        MenuManager.OpenMenu(Menu.GameUI, gameObject);
    }
}
