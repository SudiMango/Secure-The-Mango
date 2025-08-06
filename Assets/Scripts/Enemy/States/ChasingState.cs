using UnityEngine;

public class ChasingState : BaseState<EnemyController.EnemyStates, EnemyController>
{
    public ChasingState(EnemyController.EnemyStates key, EnemyController manager) : base(key, manager)
    {
    }

    public override void enterState()
    {

    }

    public override void exitState()
    {

    }

    public override void frameUpdate()
    {

    }

    public override void physicsUpdate()
    {

    }

    public override EnemyController.EnemyStates getNextState()
    {
        if (manager.playerInRange())
        {
            return EnemyController.EnemyStates.AttackingState;
        }

        return stateKey;
    }
}
