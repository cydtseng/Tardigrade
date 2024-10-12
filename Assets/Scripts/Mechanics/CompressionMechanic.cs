using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CompressionMechanic : MonoBehaviour
{
    public static string scoreKey = "SPACE::COMPRESSION";

    [SerializeField] private Transform targetObject;
    [SerializeField] public Slider progressBar;
    [SerializeField] private float compressionSpeed = 0.5f; // Speed of continuous compression
    [SerializeField] private float resistanceAmount = 0.1f;  // Resistance per spacebar press
    [SerializeField] private float maxCompression = 0.1f;    // Minimum X scale
    [SerializeField] private float maxExpansion = 1.0f;      // Maximum X scale
    [SerializeField] public float initialCompressionSpeed = 0.5f;
    [SerializeField] public float initialResistanceAmount = 0.05f;
    private bool forceInitialCompression = true;

    [SerializeField] private Typewriter typewriter;
    [SerializeField] private Player player;
    [SerializeField] private MenuManager menu;

    private float _currentScale;  // Tracks current X scale of the player
    private float _progress = 0f; // Tracks progress of resisting compression
    private float _minProgress;

    private bool isCompressionActive = false;
    private bool quickTimeCompleted = false;
    public FMODUnity.EventReference mashingSound;
    private FMOD.Studio.EventInstance instance;

    void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(mashingSound);
        _currentScale = targetObject.localScale.x;
        _minProgress = maxCompression;
        typewriter.onTypewriterComplete.AddListener(ActivateCompressionChallenge);
    }

    private void ActivateCompressionChallenge()
    {
        if (quickTimeCompleted) return;  // Prevent re-triggering if already completed

        player.ActivateQuickTimeChallenge();
        progressBar.gameObject.SetActive(true);
        isCompressionActive = true;
    }

    void Update()
    {
        if (isCompressionActive)
        {
            CompressOverTime();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                instance.start();  // grunting!
                ResistCompression();
            }
            UpdateProgressBar();
        }
    }

    private void OnDestroy()
    {
        typewriter.onTypewriterComplete.RemoveListener(ActivateCompressionChallenge);
        instance.release();
    }

    private void CompressOverTime()
    {
        // Reduce the X scale over time to simulate compression
        float _compressionSpeed = (forceInitialCompression) ? initialCompressionSpeed : compressionSpeed;
        _currentScale -= _compressionSpeed * Time.deltaTime;
        _currentScale = Mathf.Clamp(_currentScale, maxCompression, maxExpansion);
        Vector3 currentLocalScale = targetObject.localScale;
        targetObject.localScale = new Vector3(_currentScale, currentLocalScale.y, currentLocalScale.z);
        if (_currentScale <= 0.7f && _currentScale > maxCompression)
        {
            targetObject.DOShakeScale(0.2f, new Vector3(0.05f, 0, 0), vibrato: 10, randomness: 90, fadeOut: true);
        }
        if (_currentScale <= maxCompression)
        {
            Debug.Log("Compression Reached Max.");
            EndQuickTimeChallenge();
        }
        if (_currentScale < 0.3f)
        {
            forceInitialCompression = false;
        }
    }

    private void ResistCompression()
    {
        // Temporarily increase the X scale to resist compression
        _currentScale += (forceInitialCompression) ? initialResistanceAmount : resistanceAmount;
        _currentScale = Mathf.Clamp(_currentScale, maxCompression, maxExpansion);

        Vector3 currentLocalScale = targetObject.localScale;
        targetObject.localScale = new Vector3(_currentScale, currentLocalScale.y, currentLocalScale.z);
    }

    private void EndQuickTimeChallenge()
    {
        Vector3 currentLocalScale = targetObject.localScale;
        Vector3 originalScale = new Vector3(maxExpansion, currentLocalScale.y, currentLocalScale.z);
        // Scale outward past the original scale by 30% and then back to the original scale with bounce
        targetObject.DOScale(originalScale * 1.3f, 0.3f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => {
                targetObject.DOScale(originalScale, 0.3f) // Bounce back to original scale
                    .SetEase(Ease.OutBounce);
                // Deactivate the quick-time event when compression completes
                isCompressionActive = false;
                player.DeactivateQuickTimeChallenge();
                quickTimeCompleted = true;  // Set the flag to prevent retriggering
                player.MovePlayerToCenter();
            });

        progressBar.gameObject.SetActive(false);
        menu.BumpPlayer();
        Debug.Log("Quick-time challenge complete!");
    }

    private void UpdateProgressBar()
    {
        // Update the progress bar value based on the current scale
        _progress = Mathf.Clamp01((_currentScale - maxCompression) / (maxExpansion - maxCompression));
        progressBar.value = _progress;
        _minProgress = Mathf.Min(_minProgress, _progress);

        // End the challenge if the progress bar is full
        if (_progress >= 1.0f)
        {
            PersistentState.state.GetScoreManager()
                .Set(scoreKey, _minProgress);
            EndQuickTimeChallenge();
        }
    }
}
