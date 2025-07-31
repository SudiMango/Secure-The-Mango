using UnityEngine;
using UnityEngine.InputSystem;

public class PivotHandler : MonoBehaviour
{
    [SerializeField] private GameObject plr;

    // Update is called once per frame
    void Update()
    {
        // Get direction vector between mouse and pivot
        Vector3 difference = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
        difference.Normalize();

        // Get required rotation needed to point the pivot towards mouse
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        // Rotate the pivot towards mouse based on which side player is facing
        if (plr.transform.localScale.x > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, rotZ);
        }
        else if (plr.transform.localScale.x < 0)
        {
            transform.localRotation = Quaternion.Euler(180, 180, -rotZ);
        }

        // Cap how much the pivot can rotate so it can't point backwards
        Vector3 euler = transform.eulerAngles;
        if (euler.z > 180)
        {
            euler.z -= 360;
        }
        euler.z = Mathf.Clamp(euler.z, -90, 90);
        transform.eulerAngles = euler;
    }
}
