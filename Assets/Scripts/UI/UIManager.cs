using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] UIButtonObject files;
    [SerializeField] UIButtonObject terminal;
    [SerializeField] UIButtonObject network;
    [SerializeField] UIButtonObject menu;
    Dictionary<UIState, UIButtonObject> buttons;

    UIState currentState = UIState.Gameplay;

    public static event Action<UIState> StateChanged; //NEED?

    private void OnEnable()
    {
        buttons = new Dictionary<UIState, UIButtonObject>() {
            {UIState.Files, files},
            {UIState.Terminal, terminal},
            {UIState.Network, network},
            { UIState.Menu, menu},
            };
    }

    public void SetThisStateCurrent(UIState upcomingState)
    {
        if (buttons.ContainsKey(currentState))
        {
            buttons[currentState].UnpressButton();
        }
        if (buttons.ContainsKey(upcomingState))
        {
            buttons[upcomingState].PressButton();
        }
        ImplementUpcomingState(upcomingState);
        currentState = upcomingState;
    }

    public void ImplementUpcomingState(UIState upcomingState)
    {
        StateChanged?.Invoke(upcomingState);
        GameObject.FindObjectOfType<MenuBackground>().SetState(upcomingState, currentState);
    }

    public bool CheckCurrentState(UIState state)
    {
        return state == currentState;
    }
}