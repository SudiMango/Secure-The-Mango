using UnityEngine;
using UnityEngine.UI;

// EnemyUIHandler manages all enemy UI
public class EnemyUIHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform owner;
    [SerializeField] private RectTransform healthPanel;
    private Transform healthBar;

    [Header("Events")]
    [SerializeField] private GameEvent onHealthChanged;

    private void Awake()
    {
        // Fetch all sub-components

        healthBar = healthPanel.Find("HealthBar");
    }

    // MODIFIES: self
    // EFFECTS: updates health bar and text based on currentHealth and maxHealth
    public void updateHealthBar(Component sender, object data)
    {
        if (sender.gameObject.CompareTag("Player") || !sender.Equals(owner)) return;

        float[] _data = data as float[];
        float currentHealth = _data[0];
        float maxHealth = _data[1];

        healthBar.GetComponent<Slider>().value = currentHealth / maxHealth;
        healthBar.Find("Fill").GetComponent<Image>().color = healthToColor(currentHealth, maxHealth);
    }

    // EFFECTS: returns color based on enemy health
    private Color healthToColor(float currentHealth, float maxHealth)
    {
        float rVal;
        float gVal;

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
    // EFFECTS: flips the canvas on the x-axis
    public void flipCanvas()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
