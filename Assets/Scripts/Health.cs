using UnityEngine;
using UnityEngine.UI;

// Health script manages all health related stuff
public class Health : MonoBehaviour
{
    [Header("Health settings")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

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

    // REQUIRES: attached gameobject to have tag Enemy or Player
    // MODIFIES: UIManager
    // EFFECTS: updates health UI depending on if entity is player or enemy
    private void updateUI()
    {
        if (gameObject.CompareTag("Enemy"))
        {
            transform.GetComponent<EnemyUIManager>().updateHealthBar(currentHealth, maxHealth);
        }
        else if (gameObject.CompareTag("Player"))
        {
            UIManager.getInstance().updateHealthBar(currentHealth, maxHealth);
        }
    }

}
