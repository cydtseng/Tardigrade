using System;
using System.Collections.Generic;
using UnityEngine;

public class CreditsController : MonoBehaviour
{
    // Transition
    public string nextSceneName;

    private void Start()
    {
        CalculateScore();
    }

    private float CalculateScore() {
        ScoreManager scorer = PersistentState.state.GetScoreManager();
        foreach (KeyValuePair<string, float> kv in scorer.playRecord) {
            Debug.LogFormat("{0}: {1}", kv.Key, kv.Value);
        }
        return 0f;
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
