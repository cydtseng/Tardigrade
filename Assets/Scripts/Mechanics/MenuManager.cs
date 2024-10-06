using UnityEngine;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    public RectTransform canvas;
    public RectTransform titleText;  
    public Transform player;         
    private Vector2 canvasLocalCenter;
    
    void Start()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        // Convert screen center to local coordinates relative to the canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenCenter, null, out canvasLocalCenter);
        Vector2 startPosition = new Vector2(canvasLocalCenter.x, -(Screen.height * 2));
        titleText.anchoredPosition = startPosition;
    }
    
    public void BumpPlayer()
    {
        titleText.DOAnchorPos(canvasLocalCenter, 1f)
            .SetEase(Ease.OutQuad) 
            .OnComplete(() => {
                Vector3 bumpDirection = new Vector3(0f, 2f, 0f);
                player.DOMove(player.position + bumpDirection, 0.5f)
                    .SetEase(Ease.OutBack);
            });
    }
    
    public void StartGame()
    {
        titleText.DOAnchorPos(new Vector2(canvasLocalCenter.x, Screen.height * 3), 1f)
            .SetEase(Ease.InQuad)
            .OnComplete(() => {
                Debug.Log("Game Started!");
                // SceneManager.LoadScene("GameScene");
                Initiate.Fade("Volcano", Color.black,1);
            });
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // Freeze game logic
        Debug.Log("Game Paused!");
    }
    
    public void ContinueGame()
    {
        Time.timeScale = 1f; // Resume game logic
        Debug.Log("Game Resumed!");
    }
    
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
