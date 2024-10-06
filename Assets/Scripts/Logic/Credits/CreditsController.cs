using System;
using UnityEngine;

public class CreditsController : MonoBehaviour
{
    private void Start()
    {
        
    }

    public void StartGame()
    {
        Initiate.Fade("Space", Color.black,1);
    }
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
