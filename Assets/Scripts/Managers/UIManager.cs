using UnityEngine;
using UnityEngine.UI;

// UIManager manages all player UI
public class UIManager : Singleton<UIManager>
{

    // Weapon info
    public RectTransform WeaponInfo;
    private Transform magazineText;
    private Transform totalAmmoText;
    private Transform reloadSlider;

    // Health
    public RectTransform HealthPanel;
    private Transform healthBar;
    private Transform healthText;

    private void Start()
    {
        // Fetch all sub-components

        magazineText = WeaponInfo.Find("Magazine");
        totalAmmoText = WeaponInfo.Find("TotalAmmo");
        reloadSlider = WeaponInfo.Find("ReloadSlider");

        healthBar = HealthPanel.Find("HealthBar");
        healthText = HealthPanel.Find("HealthText");
    }

    #region Health

    // MODIFIES: self
    // EFFECTS: updates health bar and text based on currentHealth and maxHealth
    public void updateHealthBar(float currentHealth, float maxHealth)
    {
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(healthToSize(currentHealth, maxHealth), 70);
        healthBar.GetComponent<Image>().color = healthToColor(currentHealth, maxHealth);
        healthText.GetComponent<Text>().text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }

    // EFFECTS: returns x size of healthBar depending on player health
    private float healthToSize(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth * 500;
    }

    // EFFECTS: returns color based on player health
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

    #endregion

    #region Weapon info

    // MODIFIES: self
    // EFFECTS: updates magazine text
    public void updateMagazine(int currentAmmo, int magazineCapacity)
    {
        magazineText.GetComponent<Text>().text = currentAmmo.ToString() + "/" + magazineCapacity.ToString();
    }

    // MODIFIES: self
    // EFFECTS: updates totalAmmo text
    public void updateTotalAmmo(int totalAmmo)
    {
        totalAmmoText.GetComponent<Text>().text = totalAmmo.ToString();
    }

    // MODIFIES: self
    // EFFECTS: updates length of reloadSlider
    public void updateReloadSlider(float value)
    {
        reloadSlider.GetComponent<Slider>().value = value;
    }

    #endregion
}