using UnityEngine;
using UnityEngine.UI;

// Health script manages all health related stuff
public class Health : MonoBehaviour
{
    [Header("Health settings")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    [Space]

    [SerializeField] private bool isEnemy = false;

    void Start()
    {
        // Set initial health values
        currentHealth = maxHealth;
        updateUI();
    }

    // MODIFIES: self, UIManager
    // EFFECTS: sets currentHealth to new value
    public void setCurrentHealth(float currentHealth)
    {
        this.currentHealth = currentHealth;
        updateUI();
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
        updateUI();
    }

    // EFFECTS: returns the value of maxHealth
    public float getMaxHealth()
    {
        return maxHealth;
    }

    // MODIFIES: UIManager
    // EFFECTS: updates health UI depending on if entity is player or enemy
    private void updateUI()
    {
        if (isEnemy)
        {
            transform.GetComponent<EnemyUIManager>().updateHealthBar(currentHealth, maxHealth);
        }
        else
        {
            UIManager.getInstance().updateHealthBar(currentHealth, maxHealth);
        }
    }

}
