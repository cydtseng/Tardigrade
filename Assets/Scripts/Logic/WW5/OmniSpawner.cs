using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmniSpawner : MonoBehaviour
{
    public GameObject leftSpawnObject;
    public GameObject rightSpawnObject;
    public GameObject topSpawnObject;
    public GameObject bottomSpawnObject;

    public float spawnProb = 0.5f;
    public float spawnIntervalSeconds = 0.1f;
    public float spawnMargin = 0.5f;
    private int triggerCountdown;

    void Start() {
        triggerCountdown = (int)(50 * spawnIntervalSeconds);
    }

    void FireEvent()
    {
        Camera camera = Camera.main;
        float hh = camera.orthographicSize;  // half-height
        float h = 2 * hh;
        float hw = hh * camera.aspect; // half-width
        float w = h * camera.aspect;

        // Sample uniformly from all directions
        // [LEFT:h][TOP:w]0[BOTTOM:w][RIGHT:h]
        float r = Random.Range(-h-w, h+w);
        float x, y;
        GameObject spawnObject;
        if ((r < w) && (r > -w)) {
            // Top or bottom
            x = Random.Range(-hw, hw);
            y = hh + spawnMargin;
            if (r > 0) {
                spawnObject = bottomSpawnObject;
                y = -y;
            } else {
                spawnObject = topSpawnObject;
            }
        } else {
            // Left or right
            x = hw + spawnMargin;
            y = Random.Range(-hh, hh);
            if (r > 0) {
                spawnObject = rightSpawnObject;
            } else {
                spawnObject = leftSpawnObject;
                x = -x;
            }
        }
        GameObject obj = Instantiate(
            spawnObject, new Vector3(x, y, 0), Quaternion.identity, this.transform
        );
        obj.SetActive(true);
    }

    void FixedUpdate()
    {
        triggerCountdown--;
        if (triggerCountdown < 0) {
            triggerCountdown = (int)(50 * spawnIntervalSeconds);
            if (Random.Range(0f, 1f) > spawnProb) return;
            FireEvent();
        }
    }
}
