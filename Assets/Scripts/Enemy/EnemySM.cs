using UnityEngine;

public class EnemySM : StateManager<EnemySM.EnemyStates>
{
    public enum EnemyStates
    {
        PatrollingState,
        ChasingState,
        AttackingState
    }

    void Awake()
    {
        states.Add(EnemyStates.PatrollingState, new PatrollingState(EnemyStates.PatrollingState));
        states.Add(EnemyStates.ChasingState, new ChasingState(EnemyStates.ChasingState));
        states.Add(EnemyStates.AttackingState, new AttackingState(EnemyStates.AttackingState));

        currentState = states[EnemyStates.PatrollingState];
    }
}
