using UnityEngine;

public class HealthPack : PickableItem
{
    // Variables

    [SerializeField] private float percentToHealLower = 0.1f;
    [SerializeField] private float percentToHealUpper = 0.3f;

    // MODIFIES: self
    // EFFECTS: fires when player picks up the item
    public override void onPickup()
    {
        onItemPickup.raise(this, Random.Range(percentToHealLower, percentToHealUpper));
        Destroy(gameObject);
    }
}
