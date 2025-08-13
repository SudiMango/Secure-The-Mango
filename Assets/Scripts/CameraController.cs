using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera settings")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [Header("Camera shake settings")]
    [SerializeField] private AnimationCurve curve;
    public float duration;

    [Header("Events")]
    [SerializeField] private GameEvent onAttack;

    // Update camera position to player
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

    // MODIFIES: camera
    // EFFECTS: method called to shake the camera
    public void shakeCamera(Component sender, object data)
    {
        if (!sender.gameObject.CompareTag("Player")) return;

        StopCoroutine(shake());
        StartCoroutine(shake());
    }

    // MODIFIES: camera
    // EFFECTS: shakes the camera
    private IEnumerator shake()
    {
        Vector3 startPos = transform.localPosition;
        float elapsedTime = 0;

        while (elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.localPosition = startPos + Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.localPosition = startPos;
    }
}
