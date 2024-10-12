using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTrigger : MonoBehaviour
{
    public GameObject target;
    public GameObject splatter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == target)
        {
            Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
            Vector2 collisionPoint = rb.position;
            
            // Get the closest point on the collider to find the surface normal
            Vector2 closestPoint = other.ClosestPoint(collisionPoint);
            Vector2 surfaceNormal = (closestPoint - collisionPoint).normalized;
            
            float angle = Mathf.Atan2(surfaceNormal.y, surfaceNormal.x) * Mathf.Rad2Deg;

            GameObject obj = Instantiate(splatter, collisionPoint, Quaternion.Euler(0, 0, angle + 90));

            obj.SetActive(true);
            Destroy(gameObject);
        }
    }
}
