using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    #region Variables

    // Public variables
    public WeaponDataScriptableObject Data => data;

    [Header("References")]
    [SerializeField] protected WeaponDataScriptableObject data;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected Transform owner;

    // Timers
    protected float primaryTimer = 0f;
    protected float secondaryTimer = 0f;

    // Other
    protected bool canAttack = true;

    #endregion

    #region Unity callback functions

    protected virtual void Start()
    {
        // Set default values
        primaryTimer = data.timeBetweenPrimaryFire;
        secondaryTimer = data.timeBetweenSecondaryFire;
    }

    protected virtual void Update()
    {
        // Update timers
        primaryTimer += Time.deltaTime;
        secondaryTimer += Time.deltaTime;
    }

    #endregion

    #region Custom functions

    // EFFECTS: primary method of attack
    public abstract void onPrimaryFire();

    // EFFECTS: secondary method of attack
    public abstract void onSecondaryFire();

    // EFFECTS: returns the direction depending on x localscale of root entity
    protected int getDir()
    {
        if (owner.localScale.x > 0)
        {
            return 1;
        }

        return -1;
    }

    #endregion
}
