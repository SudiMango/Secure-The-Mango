using System;
using UnityEngine;

public class PatrollingState : BaseState<EnemyController.EnemyStates, EnemyController>
{
    // Variables

    private float startingPosX;

    // EFFECTS: create new state
    public PatrollingState(EnemyController.EnemyStates key, EnemyController manager) : base(key, manager) { }

    // EFFECTS: called when entering the state
    public override void enterState()
    {
        startingPosX = manager.getParent().transform.position.x;
    }

    // EFFECTS: called when exiting the state
    public override void exitState() { }

    // EFFECTS: called on default Update method
    public override void frameUpdate()
    {
        if (Math.Abs(manager.getParent().transform.position.x - startingPosX) >= manager.data.patrolWalkRange
            || !manager.isNearGround()
            || manager.isAgainstWall())
        {
            manager.flip();
            startingPosX = manager.getParent().transform.position.x;
        }
    }

    // EFFECTS: called on default FixedUpdate method
    public override void physicsUpdate()
    {
        manager.rb.linearVelocityX = manager.data.patrolSpeed * manager.getDir();
    }

    // EFFECTS: returns next state if conditions are met, otherwise returns current state
    public override EnemyController.EnemyStates getNextState()
    {
        if (manager.playerInRange())
        {
            return EnemyController.EnemyStates.ChasingState;
        }

        return stateKey;
    }
}
