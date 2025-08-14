using UnityEngine;

public class EnemyController : StateManager<EnemyController.EnemyStates, EnemyController>
{
    public EnemyDataScriptableObject data;

    public Gun currentGun;
    public Rigidbody2D rb;
    public bool facingRight = true;

    [Header("References")]
    [SerializeField] private Transform edgeCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    // Enemy states
    public enum EnemyStates
    {
        PatrollingState,
        ChasingState,
        AttackingState
    }

    // Initialize all the states
    void Awake()
    {
        states.Add(EnemyStates.PatrollingState, new PatrollingState(EnemyStates.PatrollingState, this));
        states.Add(EnemyStates.ChasingState, new ChasingState(EnemyStates.ChasingState, this));
        states.Add(EnemyStates.AttackingState, new AttackingState(EnemyStates.AttackingState, this));

        currentState = states[EnemyStates.PatrollingState];
    }

    #region Custom functions

    // EFFECTS: returns true if there is still ground in front of the enemy
    public bool isNearGround()
    {
        return Physics2D.OverlapCircle(edgeCheck.position, 0.2f, groundLayer);
    }

    // EFFECTS: returns true if the enemy is against a wall
    public bool isAgainstWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer) && GetComponent<Rigidbody2D>().linearVelocityX < float.Epsilon;
    }

    // EFFECTS: returns true if player is in range
    public bool playerInRange(float range)
    {
        if (Vector3.Distance(transform.position, PlayerManager.getInstance().getPosition()) < range)
        {
            return true;
        }

        return false;
    }

    // EFFECTS: returns true if player is in line of sight
    public bool playerInLOS()
    {
        Vector3 dir = PlayerManager.getInstance().getPosition() - (Vector2)transform.position;
        dir.Normalize();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 20, ~LayerMask.GetMask("Bullet", "OneWayPlatform", "Enemy", "Ladder"));

        if (hit.collider != null)
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    // EFFECTS: returns true if player is in range and in line of sight
    public bool playerInRangeAndLOS(float range)
    {
        return playerInRange(range) && playerInLOS();
    }

    // MODIFIES: transform.localScale
    // EFFECTS: flips the character on the x-axis
    public void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
        transform.Find("EnemyCanvas").GetComponent<EnemyUIHandler>().flipCanvas();
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

    // EFFECTS: returns the original enemy gameObject
    public GameObject getParent()
    {
        return gameObject;
    }

    #endregion
}
