using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class OmniSpawner : MonoBehaviour
{
    // Main game logic
    public GameObject leftSpawnObject;
    public GameObject rightSpawnObject;
    public GameObject topSpawnObject;
    public GameObject bottomSpawnObject;

    public float spawnProb = 0.5f;
    public float spawnIntervalSeconds = 0.1f;
    public float spawnMargin = 0.5f;
    private int triggerCountdown;

    // Transition and feedback logic
    public Typewriter typewriter;
    public Slider progressBar;
    private bool startGeneratingRadiation = false;
    private float spawnTime = 20f; // Total duration for spawning fireballs for now
    private float elapsedTime = 0f;
    public string nextSceneName;  // Set the next scene's name in the Unity Inspector

    void Start()
    {
        progressBar.gameObject.SetActive(false);
        triggerCountdown = (int)(50 * spawnIntervalSeconds);
        typewriter.onTypewriterComplete.AddListener(StartSpawn);
        progressBar.maxValue = spawnTime;
        progressBar.value = spawnTime; // Start with full progress bar
    }

    private void StartSpawn()
    {
        progressBar.gameObject.SetActive(true);
        startGeneratingRadiation = true;
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
        if (startGeneratingRadiation)
        {
            elapsedTime += Time.fixedDeltaTime;
            progressBar.value = Mathf.Max(spawnTime - elapsedTime, 0); // Update the progress bar

            triggerCountdown--;
            if (triggerCountdown < 0) {
                triggerCountdown = (int)(50 * spawnIntervalSeconds);
                if (Random.Range(0f, 1f) > spawnProb) return;
                FireEvent();
            }

            // Stop spawning after 20 seconds
            if (elapsedTime >= spawnTime)
            {
                startGeneratingRadiation = false;
                progressBar.gameObject.SetActive(false);

                // Perform next scene fadeout and transition immediately
                StartCoroutine(FadeAndLoadScene());
            }
        }
    }

    IEnumerator FadeAndLoadScene()
    {
        Initiate.Fade(nextSceneName, Color.black, 1);
        yield break;
    }
}
