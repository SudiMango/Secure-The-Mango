using UnityEngine;
using UnityEngine.UI;

// EnemyUIManager manages all enemy UI
public class EnemyUIManager : MonoBehaviour
{
    [SerializeField] public RectTransform healthPanel;
    private Transform healthBar;

    private void Awake()
    {
        // Fetch all sub-components

        healthBar = healthPanel.Find("HealthBar");
    }

    // MODIFIES: self
    // EFFECTS: updates health bar and text based on currentHealth and maxHealth
    public void updateHealthBar(float currentHealth, float maxHealth)
    {
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(healthToSize(currentHealth, maxHealth), 70);
        healthBar.GetComponent<Image>().color = healthToColor(currentHealth, maxHealth);
    }

    // EFFECTS: returns x size of healthBar depending on enemy health
    private float healthToSize(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth * 500;
    }

    // EFFECTS: returns color based on enemy health
    private Color healthToColor(float currentHealth, float maxHealth)
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
}
