using UnityEngine;

public class Invincibility : PickableItem
{
    // Variables

    [SerializeField] private float invincibleTimeLower = 2f;
    [SerializeField] private float invincibleTimeUpper = 5f;

    // MODIFIES: self
    // EFFECTS: fires when player picks up the item
    public override void onPickup()
    {
        onItemPickup.raise(this, Random.Range(invincibleTimeLower, invincibleTimeUpper));
        Destroy(gameObject);
    }
}
