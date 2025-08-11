public class ChasingState : BaseState<EnemyController.EnemyStates, EnemyController>
{
    // EFFECTS: create new state
    public ChasingState(EnemyController.EnemyStates key, EnemyController manager) : base(key, manager)
    {
        behavior = manager.getParent().GetComponent<ChaseBehavior>();
    }

    // EFFECTS: called when entering the state
    public override void enterState()
    {
        behavior.onEnterState(manager);
    }

    // EFFECTS: called when exiting the state
    public override void exitState()
    {
        behavior.onExitState(manager);
    }

    // EFFECTS: called on default Update method
    public override void frameUpdate()
    {
        behavior.onFrameUpdate(manager);
    }

    // EFFECTS: called on default FixedUpdate method
    public override void physicsUpdate()
    {
        behavior.onPhysicsUpdate(manager);
    }

    // EFFECTS: returns next state if conditions are met, otherwise returns current state
    public override EnemyController.EnemyStates getNextState()
    {
        return behavior.onGetNextState(manager, stateKey);
    }
}
