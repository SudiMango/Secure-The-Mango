using UnityEngine;

public class AmmoRefill : PickableItem
{
    // Variables

    [SerializeField] private int amountToRefillLower = 5;
    [SerializeField] private int amountToRefillUpper = 20;

    // MODIFIES: self
    // EFFECTS: fires when player picks up the item
    public override void onPickup()
    {
        onItemPickup.raise(this, Random.Range(amountToRefillLower, amountToRefillUpper));
        Destroy(gameObject);
    }
}
