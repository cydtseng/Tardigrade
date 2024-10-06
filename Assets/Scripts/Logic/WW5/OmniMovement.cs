using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmniMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public int directionUDLR = 0;  // default down
    public float moveSpeed = 20f;
    public float despawnMargin = 0.5f;
    public float angleSpreadRadians = 0f;

    private Vector2 direction;
    private Vector2 directionCorr;
    private float destroyPosition = 0;

    void Start() {
        Camera camera = Camera.main;
        rb = this.GetComponent<Rigidbody2D>();
        float hh = camera.orthographicSize;  // half-height
        float hw = hh * camera.aspect; // half-width
        float angle = Random.Range(-angleSpreadRadians, angleSpreadRadians);
        if (directionUDLR == 0) {  // up
            direction = Vector2.up;
            directionCorr = new Vector2(angle, 0);  // TODO: Need to free before destroy?
            destroyPosition = hh + despawnMargin;
        } else if (directionUDLR == 1) {
            direction = Vector2.down;
            directionCorr = new Vector2(angle, 0);
            destroyPosition = -hh - despawnMargin;
        } else if (directionUDLR == 2) {
            direction = Vector2.left;
            directionCorr = new Vector2(0, angle);
            destroyPosition = -hw - despawnMargin;
        } else {
            direction = Vector2.right;
            directionCorr = new Vector2(0, angle);
            destroyPosition = hw + despawnMargin;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + (direction + directionCorr) * moveSpeed * Time.fixedDeltaTime);
        if (direction.y != 0) {
            if (direction.y == 1) {  // up
                if (rb.position.y > destroyPosition) Destroy(gameObject);
            } else {  // down
                if (rb.position.y < destroyPosition) Destroy(gameObject);
            }
        } else {
            if (direction.x == 1) {  // right
                if (rb.position.x > destroyPosition) Destroy(gameObject);
            } else {  // left
                if (rb.position.x < destroyPosition) Destroy(gameObject);
            }
        }
    }
}
