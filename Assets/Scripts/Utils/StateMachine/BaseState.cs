using System;

// BaseState handles the abstraction of every unique state
public abstract class BaseState<TState> where TState : Enum
{
    // Variables

    public TState stateKey { get; private set; }

    // EFFECTS: creates new state with its unique key
    public BaseState(TState key)
    {
        stateKey = key;
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
