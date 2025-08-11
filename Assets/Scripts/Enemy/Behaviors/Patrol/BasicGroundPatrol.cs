using System;

public class BasicGroundPatrol : PatrolBehavior
{
    // Variables

    private float startingPosX;

    // EFFECTS: called when entering the state
    public override void onEnterState(EnemyController manager)
    {
        startingPosX = manager.getParent().transform.position.x;
    }

    // EFFECTS: called when exiting the state
    public override void onExitState(EnemyController manager) { }

    // EFFECTS: called on default Update method
    public override void onFrameUpdate(EnemyController manager)
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
    public override void onPhysicsUpdate(EnemyController manager)
    {
        manager.rb.linearVelocityX = manager.data.patrolSpeed * manager.getDir();
    }

    // EFFECTS: returns next state if conditions are met, otherwise returns current state
    public override EnemyController.EnemyStates onGetNextState(EnemyController manager, EnemyController.EnemyStates stateKey)
    {
        if (manager.playerInRangeAndLOS(manager.data.playerDetectionRange))
        {
            return EnemyController.EnemyStates.ChasingState;
        }

        return stateKey;
    }
}
