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

    UIState currentState;
    //UIState previousState;

    public static event Action<UIState> StateChanged;

    private void OnEnable()
    {
        buttons = new Dictionary<UIState, UIButtonObject>() {
            {UIState.Files, files},
            {UIState.Terminal, terminal},
            {UIState.Network, network},
            { UIState.Menu, menu},
            };
        // foreach (KeyValuePair<UIState, UIButtonObject> kvp in buttons)
        // {
        //     kvp.Value.thisState = kvp.Key;
        // }
    }

    public void SetThisStateCurrent(UIState upcomingState)
    {   
        //previousState = currentState;

        if (currentState == UIState.Gameplay)
        {
            buttons[upcomingState].PressButton();
            currentState = upcomingState;
        }
        else if (upcomingState == UIState.Gameplay)
        {
            buttons[currentState].UnpressButton();
            currentState = upcomingState;
        }
        else
        {
            buttons[currentState].UnpressButton();
            currentState = upcomingState;
            buttons[currentState].PressButton();
        }
        ImplementCurrentState();
    }

    public void ImplementCurrentState()
    {
        StateChanged?.Invoke(currentState);
    }

    public bool CheckCurrentState(UIState state)
    {
        return state == currentState;
    }
}