using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    // References
    private Rigidbody2D rb;
    private string enemyTag;

    // Bullet settings
    private float speed;
    private float damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get rigidbody of bullet
        rb = GetComponent<Rigidbody2D>();

        // Set bullet speed
        rb.linearVelocity = transform.right * speed * getDir();

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

    // MODIFIES: self
    // EFFECTS: sets the speed the bullet should travel in
    public void setSpeed(float speed)
    {
        this.speed = speed;
    }

    // MODIFIES: enemy
    // EFFECTS: damages enemy by certain amount
    private void damageEnemy(GameObject enemy)
    {
        Health h = enemy.GetComponent<Health>();
        h.setCurrentHealth(h.getCurrentHealth() - damage);
    }

    // EFFECTS: returns the direction based on player's x localscale
    private int getDir()
    {
        if (GameObject.Find("Player").transform.localScale.x < 0)
        {
            return -1;
        }
        return 1;
    }
}
