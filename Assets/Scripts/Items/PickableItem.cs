using UnityEngine;

public abstract class PickableItem : MonoBehaviour
{
    // Variables

    protected Rigidbody2D rb;
    private float maxForceX = 4;
    private float maxForceY = 10;

    [SerializeField] private ItemType itemType;

    [Header("Events")]
    [SerializeField] protected GameEvent onItemPickup;

    // All item types in the game
    public enum ItemType
    {
        HealthPack,
        AmmoRefill,
        Invincibility,
    }

    #region Unity callback functions

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Add initial force when item is spawned
        Vector2 force = Vector2.zero;
        force.x = Random.Range(-maxForceX, maxForceX);
        force.y = maxForceY;

        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            onPickup();
        }
    }

    #endregion

    #region Custom functions

    // MODIFIES: self
    // EFFECTS: fires when player picks up the item
    public abstract void onPickup();

    // EFFECTS: returns the item type of the current item
    public ItemType getItemType()
    {
        return itemType;
    }

    #endregion
}
