using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float padForce;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            boost(collision.gameObject);
        }
    }

    private void boost(GameObject player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.up * padForce, ForceMode2D.Impulse);
    }
}
