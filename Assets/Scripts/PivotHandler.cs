using UnityEngine;
using UnityEngine.InputSystem;

public class PivotHandler : MonoBehaviour
{

    [SerializeField] private Transform armPivot;
    [SerializeField] public bool _canPivot = false;

    void Update()
    {
        /* 
            Determining direction 
        
                For player
                mouse position - player position
        
                For enemy
                player position - enemy position
        */

        if (_canPivot)
        {
            if (gameObject.CompareTag("Player"))
            {
                setPivot(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), transform.position);
            }
            else if (gameObject.CompareTag("Enemy"))
            {
                setPivot(PlayerManager.getInstance().getPosition(), transform.position);
            }
        }
    }

    public bool canPivot
    {
        get { return canPivot; }
        set { _canPivot = value; }
    }

    // MODIFIES: armPivot
    // EFFECTS: sets rotation of armPivot depending on pos2 and pos1
    public void setPivot(Vector3 pos2, Vector3 pos1)
    {
        // Get direction vector
        Vector3 difference = pos2 - pos1;
        difference.Normalize();

        // Get required rotation needed to point the pivot towards dir
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        // Rotate the pivot towards target based on which side entity is facing
        if (transform.localScale.x > 0)
        {
            armPivot.localRotation = Quaternion.Euler(0, 0, rotZ);
        }
        else if (transform.localScale.x < 0)
        {
            armPivot.localRotation = Quaternion.Euler(180, 180, -rotZ);
        }

        // Cap how much the pivot can rotate so it can't point backwards
        Vector3 euler = armPivot.eulerAngles;
        if (euler.z > 180)
        {
            euler.z -= 360;
        }
        euler.z = Mathf.Clamp(euler.z, -90, 90);
        armPivot.eulerAngles = euler;
    }

    // MODIFIES: armPivot
    // EFFECTS: resets armPivot rotation to default
    public void resetPivot()
    {
        armPivot.localRotation = Quaternion.Euler(0, 0, 0);
    }
}
