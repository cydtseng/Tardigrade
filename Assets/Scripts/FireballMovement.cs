using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float fallSpeed = 50f;
    private float destroyHeight;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        destroyHeight = -Camera.main.orthographicSize;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + Vector2.down * fallSpeed * Time.fixedDeltaTime);
        if (rb.position.y < destroyHeight) {
            Destroy(gameObject);
        }
    }
}
