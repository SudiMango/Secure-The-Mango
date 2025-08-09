using UnityEngine;

public class AttackingState : BaseState<EnemyController.EnemyStates, EnemyController>
{
    // Variables

    // EFFECTS: create new state
    public AttackingState(EnemyController.EnemyStates key, EnemyController manager) : base(key, manager) { }

    // EFFECTS: called when entering the state
    public override void enterState()
    {
        manager.rb.linearVelocityX = 0;
        manager.getParent().GetComponent<PivotHandler>().canPivot = true;
    }

    // EFFECTS: called when exiting the state
    public override void exitState()
    {
        manager.getParent().GetComponent<PivotHandler>().canPivot = false;
        manager.getParent().GetComponent<PivotHandler>().resetPivot();
    }

    // EFFECTS: called on default Update method
    public override void frameUpdate()
    {
        if (PlayerManager.getInstance().getPosition().x < manager.getParent().transform.position.x && manager.facingRight)
        {
            manager.flip();
        }
        else if (PlayerManager.getInstance().getPosition().x > manager.getParent().transform.position.x && !manager.facingRight)
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
    public override void physicsUpdate()
    {
        manager.rb.linearVelocityX = 0;
    }

    // EFFECTS: returns next state if conditions are met, otherwise returns current state
    public override EnemyController.EnemyStates getNextState()
    {
        if (manager.playerInRange())
        {
            return stateKey;
        }

        return EnemyController.EnemyStates.PatrollingState;
    }
}
