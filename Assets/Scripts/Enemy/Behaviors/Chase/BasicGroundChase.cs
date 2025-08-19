using UnityEngine;

public class BasicGroundChase : ChaseBehavior
{
    private bool canMove = true;

    // EFFECTS: called when entering the state
    public override void onEnterState(EnemyController manager) { }

    // EFFECTS: called when exiting the state
    public override void onExitState(EnemyController manager) { }

    // EFFECTS: called on default Update method
    public override void onFrameUpdate(EnemyController manager)
    {
        float plrX = PlayerManager.getInstance().getPosition().x;
        float enemyX = manager.getParent().transform.position.x;
        if (plrX < enemyX && manager.facingRight)
        {
            if (Mathf.Abs(plrX - enemyX) >= 0.1)
            {
                manager.flip();
            }
        }
        else if (plrX > enemyX && !manager.facingRight)
        {
            if (Mathf.Abs(plrX - enemyX) >= 0.1)
            {
                manager.flip();
            }
        }

        if (Mathf.Abs(plrX - enemyX) >= 0.1)
        {
            canMove = true;
        }
        else
        {
            canMove = false;
        }
    }

    // EFFECTS: called on default FixedUpdate method
    public override void onPhysicsUpdate(EnemyController manager)
    {
        manager.rb.linearVelocityX = canMove ? manager.data.chaseSpeed * manager.getDir() : 0;
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
