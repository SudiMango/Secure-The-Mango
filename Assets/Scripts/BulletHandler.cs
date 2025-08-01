using UnityEngine;
using UnityEngine.InputSystem;

public class BulletHandler : MonoBehaviour
{
    // References
    private Rigidbody2D rb;
    private string enemyTag;

    // Bullet settings
    [SerializeField] private float speed = 5f;
    private Vector2 dir;
    private float damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get rigidbody of bullet
        rb = GetComponent<Rigidbody2D>();

        // Get direction of bullet's travel
        dir = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
        dir.Normalize();

        // Set bullet speed
        rb.linearVelocity = dir * speed;

        // Destroy self after some time
        Destroy(gameObject, 5);
    }

    // When the bullet collides with another object
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag(enemyTag))
        {
            damageEnemy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 6 || collision.gameObject.layer == 7)
        {
            if (!collision.gameObject.CompareTag("OneWayPlatform"))
            {
                Destroy(gameObject);
            }
        }
    }

    // REQUIRES: tag to be a valid tag in the game
    // MODIFIES: self
    // EFFECTS: sets the tag of the enemy that the bullet should look for
    public void setEnemyTag(string tag)
    {
        enemyTag = tag;
    }

    // MODIFIES: self
    // EFFECTS: sets the amount of damage the bullet should do
    public void setDamage(float damage)
    {
        this.damage = damage;
    }

    // MODIFIES: enemy
    // EFFECTS: damages enemy by certain amount
    private void damageEnemy(GameObject enemy)
    {
        print("damaging enemy... " + enemy.name);
    }
}
