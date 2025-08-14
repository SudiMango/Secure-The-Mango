using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelayTime;

    private Rigidbody2D rb;
    private float destroyDelayTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        destroyDelayTime = fallDelayTime + 5;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(fall());
        }
    }

    // MODIFIES: rb
    // EFFECTS: makes the platform fall and destroy itself
    private IEnumerator fall()
    {
        yield return new WaitForSeconds(fallDelayTime);
        rb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(destroyDelayTime);
        Destroy(gameObject);
    }
}
