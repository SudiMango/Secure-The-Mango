using System.Collections;
using UnityEngine;

// Health script manages all health related stuff
public class Health : MonoBehaviour
{
    // Variables

    [Header("Health settings")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    [Header("Events")]
    [SerializeField] private GameEvent onHealthChanged;
    [SerializeField] private GameEvent onDeath;

    // Other
    private bool isAlive = true;
    private bool isInvincible = false;

    #region Unity callback functions

    void Start()
    {
        // Set initial health values
        currentHealth = maxHealth;
        onHealthChanged.raise(transform, new float[] { currentHealth, maxHealth });
    }

    #endregion

    #region Custom functions

    // MODIFIES: self
    // EFFECTS: sets currentHealth to new value based on healthToAdd
    public void updateCurrentHealth(float healthToAdd)
    {
        if (healthToAdd < 0 && isInvincible) return;

        currentHealth = (currentHealth + healthToAdd >= maxHealth) ? maxHealth : currentHealth + healthToAdd;
        onHealthChanged.raise(transform, new float[] { currentHealth, maxHealth });

        if (currentHealth <= 0)
        {
            isAlive = false;
            onDeath.raise(transform, null);
        }
    }

    // EFFECTS: returns the value of currentHealth
    public float getCurrentHealth()
    {
        return currentHealth;
    }

    // MODIFIES: self
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

    // EFFECTS: returns whether entity is alive or not
    public bool getIsAlive()
    {
        return isAlive;
    }

    // MODIFIES: self
    // EFFECTS: make entity invincible for a certain period of time
    public IEnumerator becomeInvincible(float time)
    {
        isInvincible = true;
        yield return new WaitForSeconds(time);
        isInvincible = false;
    }

    #region Game event functions

    // REQUIRES: data to be of type float
    // MODIFIES: health
    // EFFECTS: adds health to player when health pack is picked up
    public void onHealthPack(Component sender, object data)
    {
        PickableItem item = sender as PickableItem;
        if (item.getItemType() != PickableItem.ItemType.HealthPack) return;

        float percent = (float)data;

        float healthToAdd = Mathf.Round(percent * getMaxHealth());
        updateCurrentHealth(healthToAdd);
    }

    // REQUIRES: data to be of type float
    // MODIFIES: health
    // EFFECTS: makes player invincible for certain period of time
    public void onInvincibility(Component sender, object data)
    {
        PickableItem item = sender as PickableItem;
        if (item.getItemType() != PickableItem.ItemType.Invincibility) return;

        float time = (float)data;
        StopCoroutine(becomeInvincible(time));
        StartCoroutine(becomeInvincible(time));
    }

    #endregion

    #endregion
}
