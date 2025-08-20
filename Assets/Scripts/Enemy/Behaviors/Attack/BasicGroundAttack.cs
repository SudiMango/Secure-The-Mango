using UnityEngine;

public class BasicGroundAttack : AttackBehavior
{
    // EFFECTS: called when entering the state
    public override void onEnterState(EnemyController manager)
    {
        manager.rb.linearVelocityX = 0;
        manager.getParent().GetComponent<PivotHandler>().canPivot = true;
    }

    // EFFECTS: called when exiting the state
    public override void onExitState(EnemyController manager)
    {
        manager.getParent().GetComponent<PivotHandler>().canPivot = false;
        manager.getParent().GetComponent<PivotHandler>().resetPivot();
    }

    // EFFECTS: called on default Update method
    public override void onFrameUpdate(EnemyController manager)
    {
        float plrX = PlayerManager.getInstance().getPosition().x;
        float enemyX = manager.getParent().transform.position.x;
        if (plrX < enemyX && manager.facingRight)
        {
            manager.flip();
        }
        else if (plrX > enemyX && !manager.facingRight)
        {
            manager.flip();
        }

        if (manager.currentGun.CurrentAmmo > 0)
        {
            manager.currentGun.onPrimaryFire();
        }
        else
        {
            manager.currentGun.onReload();
        }
    }

    // EFFECTS: called on default FixedUpdate method
    public override void onPhysicsUpdate(EnemyController manager)
    {
        manager.rb.linearVelocityX = manager.data.attackSpeed;
    }

    // EFFECTS: returns next state if conditions are met, otherwise returns current state
    public override EnemyController.EnemyStates onGetNextState(EnemyController manager, EnemyController.EnemyStates stateKey)
    {
        if (manager.playerInRangeAndLOS(manager.data.playerAttackRange))
        {
            return stateKey;
        }
        else if (manager.playerInRangeAndLOS(manager.data.playerDetectionRange))
        {
            return EnemyController.EnemyStates.ChasingState;
        }

        return EnemyController.EnemyStates.PatrollingState;
    }
}
