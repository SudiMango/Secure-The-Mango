using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    // References
    private Rigidbody2D rb;

    // Bullet settings
    private float speed;
    private float dir;
    private float damage;

    void Awake()
    {
        // Get rigidbody of bullet
        rb = GetComponent<Rigidbody2D>();

        // Destroy self after some time
        Destroy(gameObject, 5);
    }

    // When the bullet collides with another object
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
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

    // MODIFIES: self
    // EFFECTS: sets the amount of damage the bullet should do
    public void setDamage(float damage)
    {
        this.damage = damage;
    }

    // MODIFIES: self
    // EFFECTS: sets the speed the bullet should travel in
    public void setSpeed(float speed)
    {
        this.speed = speed;
    }

    // MODIFIES: self
    // EFFECTS: sets the direction of the bullet's travel
    public void setDir(int dir)
    {
        this.dir = dir;
    }

    // MODIFIES: rb
    // EFFECTS: starts the travel of the bullet
    public void startBullet()
    {
        rb.linearVelocity = transform.right * speed * dir;
    }

    // MODIFIES: enemy
    // EFFECTS: damages enemy by certain amount
    private void damageEnemy(GameObject enemy)
    {
        Health h = enemy.GetComponent<Health>();
        h.setCurrentHealth(h.getCurrentHealth() - damage);
    }
}
