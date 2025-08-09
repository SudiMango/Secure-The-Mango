using UnityEngine;

public class AttackingState : BaseState<EnemyController.EnemyStates, EnemyController>
{
    // Variables

    // References
    private GameObject parent;
    private Rigidbody2D rb;

    public AttackingState(EnemyController.EnemyStates key, EnemyController manager) : base(key, manager)
    {
    }

    public override void enterState()
    {
        parent = manager.getParent();
        rb = parent.GetComponent<Rigidbody2D>();

        rb.linearVelocityX = 0;

        parent.GetComponent<PivotHandler>().canPivot = true;
    }

    public override void exitState()
    {
        parent.GetComponent<PivotHandler>().canPivot = false;
        parent.GetComponent<PivotHandler>().resetPivot();
    }

    public override void frameUpdate()
    {
        if (PlayerManager.getInstance().getPosition().x < parent.transform.position.x && manager.facingRight)
        {
            manager.flip();
        }
        else if (PlayerManager.getInstance().getPosition().x > parent.transform.position.x && !manager.facingRight)
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

    public override void physicsUpdate()
    {
        rb.linearVelocityX = 0;
    }

    public override EnemyController.EnemyStates getNextState()
    {
        if (manager.playerInRange())
        {
            return stateKey;
        }

        return EnemyController.EnemyStates.PatrollingState;
    }
}
