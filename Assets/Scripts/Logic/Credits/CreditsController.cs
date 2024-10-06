using System;
using UnityEngine;

public class CreditsController : MonoBehaviour
{
    // Transition
    public string nextSceneName;

    private void Start()
    {

    }

    public void StartGame()
    {
        Initiate.Fade(nextSceneName, Color.black,1);
    }
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
