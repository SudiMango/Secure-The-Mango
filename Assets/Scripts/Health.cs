using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Health settings")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    [Header("UI")]
    [SerializeField] private RectTransform healthPanel;
    private Transform healthBar;
    private Transform healthText;

    void Start()
    {
        currentHealth = maxHealth;

        healthBar = healthPanel.Find("HealthBar");
        healthText = healthPanel.Find("HealthText");
    }

    void Update()
    {
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(healthToSize(), 70);
        healthBar.GetComponent<Image>().color = healthToColor();
        healthText.GetComponent<Text>().text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }

    // EFFECTS: returns x size of healthBar depending on player health
    private float healthToSize()
    {
        return currentHealth / maxHealth * 500;
    }

    // EFFECTS: returns color based on player health
    private Color healthToColor()
    {
        float rVal = 0;
        float gVal = 0;

        if (currentHealth >= maxHealth / 2)
        {
            rVal = 255 * (2 - (currentHealth / (maxHealth / 2)));
            gVal = 255;
        }
        else
        {
            rVal = 255;
            gVal = 255 * (currentHealth / (maxHealth / 2));
        }

        return new Color(rVal / 255, gVal / 255, 0 / 255);
    }

    // MODIFIES: self
    // EFFECTS: sets currentHealth to new value
    public void setCurrentHealth(float currentHealth)
    {
        this.currentHealth = currentHealth;
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
    }

    // EFFECTS: returns the value of maxHealth
    public float getMaxHealth()
    {
        return maxHealth;
    }

}
