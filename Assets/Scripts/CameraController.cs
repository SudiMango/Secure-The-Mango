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
    [SerializeField] private float duration;
    private Vector3 shakeOffset = Vector3.zero;

    [Header("Events")]
    [SerializeField] private GameEvent onShoot;

    // Update camera position to player
    void Update()
    {
        Vector3 lookAdjustedOffset = (target.localScale.x > 0) ? offset : new Vector3(offset.x * -1, offset.y, offset.z);

        Vector3 targetPosition = target.position + lookAdjustedOffset;
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        transform.position = smoothPos + shakeOffset;
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
        float elapsedTime = 0;

        while (elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            shakeOffset = Random.insideUnitSphere * strength;
            yield return null;
        }

        shakeOffset = Vector3.zero;
    }
}
