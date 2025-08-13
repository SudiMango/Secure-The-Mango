using UnityEngine;

// Health script manages all health related stuff
public class Health : MonoBehaviour
{
    [Header("Health settings")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    [Header("Events")]
    [SerializeField] private GameEvent onHealthChanged;

    void Start()
    {
        // Set initial health values
        currentHealth = maxHealth;
        onHealthChanged.raise(transform, new float[] { currentHealth, maxHealth });
    }

    // MODIFIES: self, UIManager
    // EFFECTS: sets currentHealth to new value
    public void setCurrentHealth(float currentHealth)
    {
        this.currentHealth = currentHealth;
        onHealthChanged.raise(transform, new float[] { currentHealth, maxHealth });
    }

    // EFFECTS: returns the value of currentHealth
    public float getCurrentHealth()
    {
        return currentHealth;
    }

    // MODIFIES: self, UIManager
    // EFFECTS: sets maxHealth to new value
    public void setMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
        onHealthChanged.raise(transform, new float[] { currentHealth, maxHealth });
    }

    // EFFECTS: returns the value of maxHealth
    public float getMaxHealth()
    {
        return maxHealth;
    }

}
