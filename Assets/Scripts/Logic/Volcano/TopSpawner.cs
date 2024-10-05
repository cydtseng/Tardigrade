using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopSpawner : MonoBehaviour
{
    public GameObject spawnObject;
    public float spawnProb = 0.5f;
    public float spawnIntervalSeconds = 0.1f;
    public float spawnMargin = 0.5f;
    private int triggerCountdown;

    void Start() {
        triggerCountdown = (int)(50 * spawnIntervalSeconds);
    }

    void FixedUpdate()
    {
        triggerCountdown--;
        if (triggerCountdown < 0) {
            triggerCountdown = (int)(50 * spawnIntervalSeconds);
            if (Random.Range(0f, 1f) > spawnProb) return;

            // Spawn object
            Camera camera = Camera.main;
            float left = -camera.orthographicSize * camera.aspect;
            float right = camera.orthographicSize * camera.aspect;
            GameObject obj = Instantiate(
                spawnObject,
                new Vector3(
                    Random.Range(left, right), camera.orthographicSize + spawnMargin, 0
                ),
                Quaternion.identity,
                this.transform
            );
            obj.SetActive(true);
        }
    }
}
