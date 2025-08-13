using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform HealthPanel;
    [SerializeField] private RectTransform WeaponInfo;

    // Health
    private Transform healthBar;
    private Transform healthText;

    // Weapon info
    private Transform magazineText;
    private Transform totalAmmoText;
    private Transform reloadSlider;

    private float reloadTimer = 0f;
    private float timeToReload = 0f;
    private bool isReloading = false;


    private void Awake()
    {
        // Fetch all sub-components

        healthBar = HealthPanel.Find("HealthBar");
        healthText = HealthPanel.Find("HealthText");

        magazineText = WeaponInfo.Find("Magazine");
        totalAmmoText = WeaponInfo.Find("TotalAmmo");
        reloadSlider = WeaponInfo.Find("ReloadSlider");
    }

    private void Update()
    {
        if (isReloading)
        {
            reloadTimer += Time.deltaTime;
            reloadSlider.GetComponent<Slider>().value = reloadTimer / timeToReload;
        }
    }

    #region Health

    // MODIFIES: self
    // EFFECTS: updates health bar and text based on currentHealth and maxHealth
    public void updateHealthBar(Component sender, object data)
    {
        if (!sender.gameObject.CompareTag("Player")) return;

        float[] _data = data as float[];
        float currentHealth = _data[0];
        float maxHealth = _data[1];

        healthBar.GetComponent<Slider>().value = currentHealth / maxHealth;
        healthBar.Find("Fill").GetComponent<Image>().color = healthToColor(currentHealth, maxHealth);
        healthText.GetComponent<Text>().text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }

    // EFFECTS: returns color based on player health
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

    #endregion

    #region Weapon info

    // MODIFIES: self
    // EFFECTS: updates magazine text
    public void updateMagazine(Component sender, object data)
    {
        if (!sender.gameObject.CompareTag("Player")) return;

        GunFiredEventData _data = data as GunFiredEventData;
        magazineText.GetComponent<Text>().text = _data.currentAmmo.ToString() + "/" + _data.magazineCapacity.ToString();
    }

    // MODIFIES: self
    // EFFECTS: updates totalAmmo text
    public void updateTotalAmmo(Component sender, object data)
    {
        int totalAmmo = (int)data;

        totalAmmoText.GetComponent<Text>().text = totalAmmo.ToString();
    }

    // MODIFIES: self
    // EFFECTS: updates length of reloadSlider
    public void onReloadStarted(Component sender, object data)
    {
        if (!sender.gameObject.CompareTag("Player")) return;

        float _timeToReload = (float)data;
        timeToReload = _timeToReload;
        isReloading = true;
    }

    // MODIFIES: self
    // EFFECTS: updates length of reloadSlider
    public void onReloadEnded(Component sender, object data)
    {
        if (!sender.gameObject.CompareTag("Player")) return;

        int[] _data = data as int[];
        int currentAmmo = _data[0];
        int magazineCapacity = _data[1];

        magazineText.GetComponent<Text>().text = currentAmmo.ToString() + "/" + magazineCapacity.ToString();

        isReloading = false;
        reloadTimer = 0f;
        timeToReload = 0f;
        reloadSlider.GetComponent<Slider>().value = 0;
    }

    #endregion
}
