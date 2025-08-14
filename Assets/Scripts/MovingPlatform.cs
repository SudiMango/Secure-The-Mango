using Unity.Mathematics;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Moving platform settings")]
    public float moveSpeed;
    [SerializeField] private bool isMovingRight = true;

    [Header("Checkpoints")]
    [SerializeField] private Transform leftCheckpoint;
    [SerializeField] private Transform rightCheckpoint;

    [Header("References")]
    [SerializeField] private Transform[] objectsOnPlatform;

    // Other
    private Transform currentCheckpoint;

    // Initialize current checkpoint
    void Start()
    {
        if (isMovingRight)
        {
            currentCheckpoint = rightCheckpoint;
        }
        else
        {
            currentCheckpoint = leftCheckpoint;
        }
    }

    // Constantly update platform
    void Update()
    {
        // Move platform's position towards current checkpoing
        transform.position = Vector3.MoveTowards(transform.position, currentCheckpoint.position, moveSpeed * Time.deltaTime);

        // Move all the items on top of the platform
        foreach (Transform t in objectsOnPlatform)
        {
            t.position = new Vector3(Vector3.MoveTowards(t.position, currentCheckpoint.position, moveSpeed * Time.deltaTime).x, t.position.y, t.position.z);
        }

        // Switch checkpoints when platform arrives
        if (isMovingRight && math.abs(transform.position.x + (transform.localScale.x / 2) - rightCheckpoint.position.x) < 0.1)
        {
            isMovingRight = false;
            currentCheckpoint = leftCheckpoint;
        }
        else if (!isMovingRight && math.abs(transform.position.x - (transform.localScale.x / 2) - leftCheckpoint.position.x) < 0.1)
        {
            isMovingRight = true;
            currentCheckpoint = rightCheckpoint;
        }
    }


    public int getDir()
    {
        if (isMovingRight)
        {
            return 1;
        }
        return -1;
    }
}
