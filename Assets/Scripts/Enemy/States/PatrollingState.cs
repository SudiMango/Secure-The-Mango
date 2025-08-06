using System;
using UnityEngine;

public class PatrollingState : BaseState<EnemyController.EnemyStates, EnemyController>
{
    // Variables

    // References
    private GameObject parent;
    private Rigidbody2D rb;

    // Patrolling
    public float patrolSpeed = 3f;
    public float distanceToTravel = 3f;
    public float startingPosX;

    // EFFECTS: create new state
    public PatrollingState(EnemyController.EnemyStates key, EnemyController manager) : base(key, manager) { }

    // EFFECTS: called when entering the state
    public override void enterState()
    {
        parent = manager.getParent();
        rb = parent.GetComponent<Rigidbody2D>();

        startingPosX = parent.transform.position.x;
    }

    // EFFECTS: called when exiting the state
    public override void exitState() { }

    // EFFECTS: called on default Update method
    public override void frameUpdate()
    {
        if (Math.Abs(parent.transform.position.x - startingPosX) >= distanceToTravel)
        {
            manager.flip();
            startingPosX = parent.transform.position.x;
        }
    }

    // EFFECTS: called on default FixedUpdate method
    public override void physicsUpdate()
    {
        rb.linearVelocityX = patrolSpeed * manager.getDir();
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
