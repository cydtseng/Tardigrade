using UnityEngine;

public class FireballMovement : MonoBehaviour
{
    public static string scoreKey = "VOLCANO::FIREBALL";

    private Rigidbody2D rb;
    public float fallSpeed = 20f;
    public float despawnMargin = 0.5f;
    private float destroyHeight;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        destroyHeight = -Camera.main.orthographicSize - despawnMargin;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + Vector2.down * fallSpeed * Time.fixedDeltaTime);
        if (rb.position.y < destroyHeight) {
            Destroy(gameObject);
        }
    }

    // Affect the score in the statemanager
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            PersistentState.state.GetScoreManager()
                .SetIncrement(scoreKey);
        }
    }
}
