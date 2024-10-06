using TMPro;
using UnityEngine;
using UnityEngine.UI; // Required to work with UI elements like Slider

public class Friction : WithPersistentState
{
    public SpriteRenderer silhouetteRenderer;   // Sprite renderer for the white silhouette
    public Slider heatSlider;                   // Slider UI to display heat level
    public float moveSpeed = 5f;
    public float heatRate = 0.1f;               // Rate at which heat increases with movement
    public float coolingRate = 0.05f;           // Rate at which the character cools down
    public float maxHeat = 1f;                  // Maximum heat level
    private float freezeMechanicDuration = 20f; // After 20s, progress to next level
    private float currentHeat;                  // Current heat level
    private bool isMoving = false;
    private bool isFrictionChallengeActive = false;
    public Player player;
    public Typewriter typewriter;
    private bool quickTimeCompleted = false;
    public SceneExitDetector sceneExitDetector;
    private float elapsedTime = 0f;
    public TMP_Text postQuicktimeText;
    public TMP_Text guidingArrow;

    private float minimumHeat;

    void Start()
    {
        // Initialize currentHeat to the maximum heat
        currentHeat = maxHeat;
        minimumHeat = maxHeat;

        // Start the silhouette fully transparent and deactivated
        silhouetteRenderer.gameObject.SetActive(false);
        Color silhouetteColor = silhouetteRenderer.color;
        silhouetteColor.a = 0f;
        silhouetteRenderer.color = silhouetteColor;
        
        heatSlider.gameObject.SetActive(false);
        // Initialize the slider to match the heat level
        heatSlider.value = 1f;  // Start the slider at 1 (maximum heat)
        heatSlider.minValue = 0f;
        heatSlider.maxValue = 1f;

        typewriter.onTypewriterComplete.AddListener(ActivateFrictionChallenge);
    }

    void Update()
    {
        if (isFrictionChallengeActive)
        {
            elapsedTime += Time.fixedDeltaTime;
            HandleMovement();
            HandleHeat();
            UpdateHeatSlider();
            
            // Stop the freeze friction challenge after 20s
            if (elapsedTime >= freezeMechanicDuration)
            {
                // Demo state management stuff
                state.addToScore((int)(100*(maxHeat - minimumHeat)));
                Debug.Log(state.getScore());

                isFrictionChallengeActive = false;
                heatSlider.gameObject.SetActive(false);
                postQuicktimeText.gameObject.SetActive(true);
                guidingArrow.gameObject.SetActive(true);
                sceneExitDetector.setHasSatisfiedLevel();
            }
        }
    }

    private void OnDestroy()
    {
        typewriter.onTypewriterComplete.RemoveListener(ActivateFrictionChallenge);
    }

    private void ActivateFrictionChallenge()
    {
        if (quickTimeCompleted) return;  // Prevent re-triggering if already completed
        player.ActivateQuickTimeChallenge();
        heatSlider.gameObject.SetActive(true);
        silhouetteRenderer.gameObject.SetActive(true);
        isFrictionChallengeActive = true;

        // Immediately update the slider to reflect the current heat at activation
        UpdateHeatSlider();
    }

    void HandleMovement()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            isMoving = true;
            float moveY = Mathf.Sin(Time.time * moveSpeed);
            transform.Translate(new Vector3(0, moveY, 0) * Time.deltaTime);
        }
        else
        {
            isMoving = false;
        }
    }

    void HandleHeat()
    {
        Color silhouetteColor = silhouetteRenderer.color;

        if (isMoving)
        {
            // Increase heat and reduce the alpha (making the silhouette less visible)
            currentHeat = Mathf.Clamp(currentHeat + heatRate * Time.deltaTime, 0f, maxHeat);

            // Decrease alpha value as player moves (reduce freezing effect)
            silhouetteColor.a = Mathf.Clamp(silhouetteColor.a - heatRate * Time.deltaTime, 0f, 1f);
        }
        else
        {
            // Decrease heat and increase the alpha (making the silhouette more visible)
            currentHeat = Mathf.Clamp(currentHeat - coolingRate * Time.deltaTime, 0f, maxHeat);

            // Increase alpha value as the player cools down (increase freezing effect)
            silhouetteColor.a = Mathf.Clamp(silhouetteColor.a + coolingRate * Time.deltaTime, 0f, 1f);
        }

        // Apply the updated alpha value back to the silhouette renderer
        silhouetteRenderer.color = silhouetteColor;
        minimumHeat = Mathf.Min(minimumHeat, currentHeat);
    }

    void UpdateHeatSlider()
    {
        
        heatSlider.value = currentHeat / maxHeat;  // As heat decreases, slider moves toward 0
    }
}
