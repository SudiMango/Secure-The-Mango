using System;
using UnityEngine;

public abstract class StateBehavior<TState, TManager> : MonoBehaviour
    where TState : Enum
    where TManager : MonoBehaviour
{

    // Functions

    // EFFECTS: called when entering the state
    public abstract void onEnterState(TManager manager);

    // EFFECTS: called when exiting the state
    public abstract void onExitState(TManager manager);

    // EFFECTS: called on default Update method
    public abstract void onFrameUpdate(TManager manager);

    // EFFECTS: called on default FixedUpdate method
    public abstract void onPhysicsUpdate(TManager manager);

    // EFFECTS: returns next state if conditions are met, otherwise returns current state
    public abstract TState onGetNextState(TManager manager, TState stateKey);
}
