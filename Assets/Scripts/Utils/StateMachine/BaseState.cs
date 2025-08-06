using System;
using UnityEngine;

// BaseState handles the abstraction of every unique state
public abstract class BaseState<TState, TManager>
    where TState : Enum
    where TManager : MonoBehaviour
{
    // Variables

    public TState stateKey { get; private set; }
    public TManager manager { get; set; }

    // EFFECTS: creates new state with its unique key and manager
    public BaseState(TState key, TManager manager)
    {
        stateKey = key;
        this.manager = manager;
    }

    // Abstract functions

    // EFFECTS: called when entering the state
    public abstract void enterState();

    // EFFECTS: called when exiting the state
    public abstract void exitState();

    // EFFECTS: called on default Update method
    public abstract void frameUpdate();

    // EFFECTS: called on default FixedUpdate method
    public abstract void physicsUpdate();

    // EFFECTS: returns next state if conditions are met, otherwise returns current state
    public abstract TState getNextState();
}
