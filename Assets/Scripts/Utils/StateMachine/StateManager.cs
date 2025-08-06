using System;
using System.Collections.Generic;
using UnityEngine;

// StateManager handles every state's functionality and transitioning between states
public abstract class StateManager<TState> : MonoBehaviour where TState : Enum
{
    // Variables

    protected Dictionary<TState, BaseState<TState>> states = new Dictionary<TState, BaseState<TState>>();
    protected BaseState<TState> currentState;

    protected bool isTransitioningState = false;

    // Lifecycle functions

    void Start()
    {
        currentState.enterState();
    }

    void Update()
    {
        if (isTransitioningState) return;

        // If nextState is same as currentState, keep updating. Otherwise, switch.
        TState nextStateKey = currentState.getNextState();
        if (nextStateKey.Equals(currentState.stateKey))
        {
            currentState.frameUpdate();
        }
        else
        {
            transitionToState(nextStateKey);
        }
    }

    void FixedUpdate()
    {
        if (isTransitioningState) return;

        currentState.physicsUpdate();
    }

    // MODIFIES: self
    // EFFECTS: exit current state and enter new state
    private void transitionToState(TState stateKey)
    {
        isTransitioningState = true;
        currentState.exitState();
        currentState = states[stateKey];
        currentState.enterState();
        isTransitioningState = false;
    }
}
