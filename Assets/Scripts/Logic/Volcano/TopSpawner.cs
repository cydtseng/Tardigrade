using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TopSpawner : MonoBehaviour
{
    public ObjectPooler fireballPooler; // Reference to ObjectPooler
    public float spawnProb = 0.5f;
    public float spawnIntervalSeconds = 0.1f;
    public float spawnMargin = 0.5f;
    private float spawnHorizontalMargin = 2f;
    private int triggerCountdown;
    public Typewriter typewriter;
    public Slider progressBar;
    public TMP_Text postQuicktimeText;
    public TMP_Text guidingArrow;
    private bool startGeneratingFireballs = false;
    private float fireballSpawnTime = 20f; // Total duration for spawning fireballs for now
    private float elapsedTime = 0f;
    public SceneExitDetector sceneExitDetector;


    void Start()
    {
        progressBar.gameObject.SetActive(false);
        triggerCountdown = (int)(50 * spawnIntervalSeconds);
        typewriter.onTypewriterComplete.AddListener(StartFireballSpawn);
        progressBar.maxValue = fireballSpawnTime;
        progressBar.value = fireballSpawnTime; // Start with full progress bar
    }

    private void StartFireballSpawn()
    {
        progressBar.gameObject.SetActive(false);
        startGeneratingFireballs = true;
        Camera.main.DOShakePosition(fireballSpawnTime, strength: new Vector3(0.5f, 0.5f, 0), vibrato: 10, randomness: 90, fadeOut: true);
    }

    void FixedUpdate()
    {
        if (startGeneratingFireballs)
        {
            elapsedTime += Time.fixedDeltaTime;
            progressBar.value = Mathf.Max(fireballSpawnTime - elapsedTime, 0); // Update the progress bar

            triggerCountdown--;
            if (triggerCountdown < 0)
            {
                triggerCountdown = (int)(50 * spawnIntervalSeconds);
                if (Random.Range(0f, 1f) > spawnProb) return;

                // Get a pooled fireball object
                GameObject obj = fireballPooler.GetPooledObject();
                if (obj != null)
                {
                    Camera camera = Camera.main;
                    float left = -camera.orthographicSize * camera.aspect;
                    float right = camera.orthographicSize * camera.aspect;
                    obj.transform.position = new Vector3(
                        Random.Range(left + spawnHorizontalMargin, right - spawnHorizontalMargin), camera.orthographicSize + spawnMargin, 0
                    );

                    // Randomly flip fireball along x-axis
                    if (Random.Range(0, 2) == 0) {
                        obj.GetComponent<SpriteRenderer>().flipX = true;
                        Vector3 rotation = obj.transform.rotation.eulerAngles;
                        rotation.z = -rotation.z;    // the sprite has an intrinsic rotation, so need to negate after X-flip
                        obj.transform.rotation = Quaternion.Euler(rotation);
                    }
                    obj.SetActive(true);
                }
            }

            // Stop spawning after 20 seconds
            if (elapsedTime >= fireballSpawnTime)
            {
                startGeneratingFireballs = false;
                progressBar.gameObject.SetActive(false);
                postQuicktimeText.gameObject.SetActive(true);
                guidingArrow.gameObject.SetActive(true);

                // allow user to progress to next level
                sceneExitDetector.setHasSatisfiedLevel();
            }
        }
    }
}
