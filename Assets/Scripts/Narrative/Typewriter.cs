using System;
using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Typewriter : MonoBehaviour
{
    public TMP_Text narrativeText;
    public RectTransform eKey;
    public string[] narratives; 
    public float typingSpeed = 0.05f; 
    private int currentNarrativeIndex = 0;
    private bool isTyping = false; 
    private bool isTextComplete = false; 
    private Coroutine typingCoroutine;
    private Vector3 startPosition;
    public float moveDistance = 5f; // Distance to move left and right
    public float moveDuration = 0.1f; // Duration of one complete movement cycle (left or right)
    public UnityEvent onTypewriterComplete;

    // Typing audio
    public FMODUnity.EventReference typewritingAudio;
    private FMOD.Studio.EventInstance typewritingAudioEvent;
    public FMODUnity.EventReference eKeyAudio;
    private FMOD.Studio.EventInstance eKeyAudioEvent;
    
    private Tween eKeyTween; // Store the tween reference for cleanup
    
    private void Awake()
    {
        Debug.Log(eKey.position);
    }

    void Start() {
        typewritingAudioEvent = FMODUnity.RuntimeManager.CreateInstance(typewritingAudio);
        eKeyAudioEvent = FMODUnity.RuntimeManager.CreateInstance(eKeyAudio);
        startPosition = eKey.position;
        DisplayNextNarrative();
        TweenEKey();
    }

    void Update() {
        // If the user presses E and text is typing, finish the text display immediately
        if (Input.GetKeyDown(KeyCode.E)) {
            if (eKey.gameObject.activeSelf) eKeyAudioEvent.start();
            if (isTyping) {
                StopCoroutine(typingCoroutine);
                typewritingAudioEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                narrativeText.text = narratives[currentNarrativeIndex];
                currentNarrativeIndex++;
                isTyping = false;
                isTextComplete = true;
            }
            else if (isTextComplete) {
                // Display the next narrative when text is fully displayed and E key is pressed
                DisplayNextNarrative();
            } 
        }
    }

    private void TweenEKey()
    {
        // Store the tween reference for later cleanup
        eKeyTween = eKey.DOMoveX(startPosition.x + moveDistance, moveDuration)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);  // Infinite loop with a Yoyo pattern
    }

    private void DisplayNextNarrative() {
        if (currentNarrativeIndex < narratives.Length) {
            typingCoroutine = StartCoroutine(TypeNarrative(narratives[currentNarrativeIndex]));
            typewritingAudioEvent.start();
        }
        else
        {
            eKey.gameObject.SetActive(false);
            narrativeText.text = "";
            onTypewriterComplete?.Invoke();

            // Clean up the tween when finished
            CleanUpTweens();
        }
    }

    private IEnumerator TypeNarrative(string narrative) {
        narrativeText.text = "";
        isTyping = true;
        isTextComplete = false;

        // Slowly iterate through each character
        foreach (char letter in narrative)
        {
            narrativeText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        typewritingAudioEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        isTyping = false;
        isTextComplete = true;
        currentNarrativeIndex++;
    }

    // Method to clean up tweens
    private void CleanUpTweens()
    {
        if (eKeyTween != null && eKeyTween.IsActive())
        {
            eKeyTween.Kill();  // Kill the tween and release its resources
            eKeyTween = null;
        }
    }

    // Clean up tweens when the object is destroyed
    private void OnDestroy()
    {
        CleanUpTweens();
        typewritingAudioEvent.release();
        eKeyAudioEvent.release();
    }
}
