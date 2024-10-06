using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearObject : MonoBehaviour
{
    // private Renderer renderer;
    public float disappearRate = 0.01f;

    // void Start()
    // {
    //     renderer  = GetComponent<Renderer>();
    // }

    private void FixedUpdate()
    {
        Color color = GetComponent<Renderer>().material.color;
        float newAlpha = color.a - disappearRate;
        if (newAlpha <= 0) Destroy(gameObject);
        GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, newAlpha);
    }
}
