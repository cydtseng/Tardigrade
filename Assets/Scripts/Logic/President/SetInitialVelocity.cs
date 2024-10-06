using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Relies on RigidBody2D physics for simulation, thrown towards left side
public class SetInitialVelocityAndPosition : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 10f;
    public float speedSpread = 0f;
    public float angleDeg = 45f;
    public float angleSpreadDeg = 0f;
    public float xSpread = 0f;
    public float ySpread = 0f;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();

        // Randomize position
        float dx = Random.Range(-xSpread/2, xSpread/2);
        float dy = Random.Range(-ySpread/2, ySpread/2);
        rb.MovePosition(rb.position + Vector2.left * dx + Vector2.up * dy);

        // Randomize firing angle
        angleDeg += Random.Range(-angleSpreadDeg/2, angleSpreadDeg/2);
        speed += Random.Range(-speedSpread/2, speedSpread/2);
        float horizontalSpeed = Mathf.Acos(angleDeg * Mathf.Deg2Rad) * speed;
        float verticalSpeed = Mathf.Asin(angleDeg * Mathf.Deg2Rad) * speed;
        rb.velocity = new Vector2(-horizontalSpeed, verticalSpeed);
    }
}
