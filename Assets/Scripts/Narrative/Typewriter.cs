using System;
using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

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

    private void Awake()
    {
        startPosition = eKey.transform.position;
    }

    void Start() {
        DisplayNextNarrative();
        TweenEKey();
    }

    void Update() {
        // If the user press E and text is typing, finish the text display immediately
        if (Input.GetKeyDown(KeyCode.E)) {
            if (isTyping) {
                StopCoroutine(typingCoroutine);
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
        eKey.DOMoveX(startPosition.x - moveDistance, moveDuration).SetEase(Ease.Linear)
            .OnComplete(() => {
                // Once movement to the left completes, move the object back to the right
                eKey.DOMoveX(startPosition.x + moveDistance, moveDuration).SetEase(Ease.Linear)
                    .OnComplete(() => {
                        // Repeat the left-right movement indefinitely for now
                        TweenEKey();
                    });
            });
    }

    private void DisplayNextNarrative() {
        
        if (currentNarrativeIndex < narratives.Length) {
            typingCoroutine = StartCoroutine(TypeNarrative(narratives[currentNarrativeIndex]));
        }
        else
        {
            eKey.gameObject.SetActive(false);
            narrativeText.text = "";
        }
    }

    private IEnumerator TypeNarrative(string narrative) {
        narrativeText.text = "";
        isTyping = true;
        isTextComplete = false;

        // slowly iter through each character
        foreach (char letter in narrative)
        {
            narrativeText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        isTextComplete = true;
        currentNarrativeIndex++;
    }
}