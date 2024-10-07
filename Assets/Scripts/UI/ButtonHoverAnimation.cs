using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class ButtonHoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public TextMeshProUGUI textMeshPro;
    public Color hoverColor = new Color(0.5f, 0.5f, 0.5f);
    public Color clickColor = new Color(0.3f, 0.3f, 0.3f);
    public float fadeDuration = 0.3f;

    private Color originalColor;
    private Coroutine fadeCoroutine;
    private Button button;

    void Start()
    {
        if (textMeshPro == null)
        {
            textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        }

        if (textMeshPro != null)
        {
            originalColor = textMeshPro.color;
        }
        else
        {
            Debug.LogError("ButtonHoverAnimation's TextMeshPro component not found!");
        }

        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        FadeToColor(hoverColor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        FadeToColor(originalColor);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        FadeToColor(clickColor);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(
            button.GetComponent<RectTransform>(),
            Input.mousePosition,
            null))
        {
            FadeToColor(hoverColor);
        }
        else
        {
            FadeToColor(originalColor);
        }
    }

    private void FadeToColor(Color targetColor)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeColor(targetColor));
    }

    private IEnumerator FadeColor(Color targetColor)
    {
        if (textMeshPro != null)
        {
            float elapsedTime = 0f;
            Color startColor = textMeshPro.color;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / fadeDuration);
                textMeshPro.color = Color.Lerp(startColor, targetColor, t);
                yield return null;
            }

            textMeshPro.color = targetColor;
        }
    }
}