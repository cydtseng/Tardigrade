using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PersistentState : MonoBehaviour
{
    public static PersistentState state;
    private MusicManager music;
    private ScoreManager score;

    void Awake()
    {
        if (state != null && state != this) {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        state = this;

        // Bootstrap managers
        music = GetComponent<MusicManager>();
        score = GetComponent<ScoreManager>();
    }

    public MusicManager GetMusicManager() {
        return music;
    }

    public ScoreManager GetScoreManager() {
        return score;
    }
}
