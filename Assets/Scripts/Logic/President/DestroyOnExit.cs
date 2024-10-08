using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnExitOrHit : MonoBehaviour
{
    public static string scoreKey = "PRESIDENT::EGGS";

    private Rigidbody2D rb;
    public GameObject target;
    public GameObject splatter;
    public float despawnMargin = 0.5f;
    private float xDestroy;
    private float yDestroy;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        xDestroy = -(Camera.main.orthographicSize * Camera.main.aspect) - despawnMargin;
        yDestroy = -Camera.main.orthographicSize - despawnMargin;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            PersistentState.state.GetScoreManager()
                .SetIncrement(scoreKey);
        }
        if (collision.gameObject == target) {
            GameObject obj = Instantiate(splatter, rb.position, Quaternion.identity);
            obj.SetActive(true);
            Destroy(gameObject);
        }
    }


    private void FixedUpdate()
    {
        if (rb.position.x < xDestroy) Destroy(gameObject);
        if (rb.position.y < yDestroy) Destroy(gameObject);
    }
}
