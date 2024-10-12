using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresidentSpawner : MonoBehaviour
{
    public GameObject[] spawnObjects;

    public float spawnProb = 0.5f;
    public float spawnIntervalSeconds = 0.1f;
    public float spawnMargin = 0.5f;
    private int triggerCountdown;
    public float hitProb = 0.1f;

    void Start() {
        triggerCountdown = (int)(50 * spawnIntervalSeconds);
    }

    void FireEvent(bool activatePresidentSplat = false)
    {
        GameObject spawnObject = spawnObjects[Random.Range(0, spawnObjects.Length)];
        GameObject obj = Instantiate(spawnObject);//, this.transform);
        if (!activatePresidentSplat) {
            Destroy(obj.GetComponent<DestroyOnTrigger>());
        }
        obj.SetActive(true);
    }

    void FixedUpdate()
    {
        triggerCountdown--;
        if (triggerCountdown < 0) {
            triggerCountdown = (int)(50 * spawnIntervalSeconds);
            if (Random.Range(0f, 1f) > spawnProb) return;

            // Random chance to hit the president instead
            FireEvent(Random.Range(0f, 1f) < hitProb);
        }
    }
}
