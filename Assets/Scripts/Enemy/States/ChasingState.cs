using UnityEngine;

public class ChasingState : BaseState<EnemyController.EnemyStates, EnemyController>
{
    // Variables

    // EFFECTS: create new state
    public ChasingState(EnemyController.EnemyStates key, EnemyController manager) : base(key, manager) { }

    // EFFECTS: called when entering the state
    public override void enterState()
    {
        manager.rb.linearVelocityX = 0;
    }

    // EFFECTS: called when exiting the state
    public override void exitState()
    {

    }

    // EFFECTS: called on default Update method
    public override void frameUpdate()
    {

    }

    // EFFECTS: called on default FixedUpdate method
    public override void physicsUpdate()
    {
        manager.rb.linearVelocityX = 0;
    }

    // EFFECTS: returns next state if conditions are met, otherwise returns current state
    public override EnemyController.EnemyStates getNextState()
    {
        if (manager.playerInRange())
        {
            return EnemyController.EnemyStates.AttackingState;
        }

        return stateKey;
    }
}
