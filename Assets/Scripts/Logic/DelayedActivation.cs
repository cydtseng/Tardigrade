using UnityEngine;

public class DelayedActivation : MonoBehaviour
{
    public float delayTime = 1f;
    private float elapsedTime = 0.0f;

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= delayTime) {
            foreach (Transform child in transform)
                child.gameObject.SetActive(true);
        }
    }
}