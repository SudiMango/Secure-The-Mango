using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        Vector3 lookAdjustedOffset = Vector3.zero;
        if (target.localScale.x > 0)
        {
            lookAdjustedOffset = offset;
        }
        else if (target.localScale.x < 0)
        {
            lookAdjustedOffset = new Vector3(offset.x * -1, offset.y, offset.z);
        }

        Vector3 targetPosition = target.position + lookAdjustedOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
