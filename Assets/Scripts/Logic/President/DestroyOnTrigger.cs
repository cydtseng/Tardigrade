using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTrigger : MonoBehaviour
{
    public GameObject target;
    public GameObject splatter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
        if (other.gameObject == target) {
            GameObject obj = Instantiate(splatter, rb.position, Quaternion.identity);
            obj.SetActive(true);
            Destroy(gameObject);
        }
    }
}
