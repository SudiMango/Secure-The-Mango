using UnityEngine;

public class EnemyController : StateManager<EnemyController.EnemyStates, EnemyController>
{
    public bool facingRight = true;
    public float detectionRange = 50f;

    [SerializeField] private Transform edgeCheck;
    [SerializeField] private LayerMask groundLayer;

    public Gun currentGun;

    public enum EnemyStates
    {
        PatrollingState,
        ChasingState,
        AttackingState
    }

    void Awake()
    {
        states.Add(EnemyStates.PatrollingState, new PatrollingState(EnemyStates.PatrollingState, this));
        states.Add(EnemyStates.ChasingState, new ChasingState(EnemyStates.ChasingState, this));
        states.Add(EnemyStates.AttackingState, new AttackingState(EnemyStates.AttackingState, this));

        currentState = states[EnemyStates.PatrollingState];
    }

    public GameObject getParent()
    {
        return gameObject;
    }

    public bool playerInRange()
    {
        if (Vector3.Distance(transform.position, PlayerManager.getInstance().getPosition()) < detectionRange)
        {
            Vector3 dir = PlayerManager.getInstance().getPosition() - (Vector2)transform.position;
            dir.Normalize();

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 20, ~LayerMask.GetMask("Bullet", "OneWayPlatform", "Enemy"));

            if (hit.collider != null)
            {
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    return true;
                }
            }
        }

        return false;
    }

    // MODIFIES: transform.localScale
    // EFFECTS: flips the character on the x-axis
    public void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
        transform.GetComponent<EnemyUIManager>().flipCanvas();
        facingRight = !facingRight;
    }

    // EFFECTS: returns the direction based on enemy's x localscale
    public int getDir()
    {
        if (transform.localScale.x < 0)
        {
            return -1;
        }
        return 1;
    }

    public bool isNearEdge()
    {
        return Physics2D.OverlapCircle(edgeCheck.position, 0.2f, groundLayer);
    }
}
