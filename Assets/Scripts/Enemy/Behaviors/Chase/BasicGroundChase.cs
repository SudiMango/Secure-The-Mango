public class BasicGroundChase : ChaseBehavior
{
    // EFFECTS: called when entering the state
    public override void onEnterState(EnemyController manager) { }

    // EFFECTS: called when exiting the state
    public override void onExitState(EnemyController manager) { }

    // EFFECTS: called on default Update method
    public override void onFrameUpdate(EnemyController manager)
    {
        if (PlayerManager.getInstance().getPosition().x < manager.getParent().transform.position.x && manager.facingRight)
        {
            manager.flip();
        }
        else if (PlayerManager.getInstance().getPosition().x > manager.getParent().transform.position.x && !manager.facingRight)
        {
            manager.flip();
        }
    }

    // EFFECTS: called on default FixedUpdate method
    public override void onPhysicsUpdate(EnemyController manager)
    {
        manager.rb.linearVelocityX = manager.data.chaseSpeed * manager.getDir();
    }

    // EFFECTS: returns next state if conditions are met, otherwise returns current state
    public override EnemyController.EnemyStates onGetNextState(EnemyController manager, EnemyController.EnemyStates stateKey)
    {
        if (manager.playerInRangeAndLOS(manager.data.playerAttackRange))
        {
            return EnemyController.EnemyStates.AttackingState;
        }
        else if (manager.playerInRangeAndLOS(manager.data.playerDetectionRange))
        {
            return stateKey;
        }

        return EnemyController.EnemyStates.PatrollingState;
    }
}
