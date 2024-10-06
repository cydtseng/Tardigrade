using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int scoreCounter = 0;

    public void IncrementScore() {
        scoreCounter += 1;
    }

    public int GetScore() {
        return scoreCounter;
    }

    public void AddToScore(int value) {
        scoreCounter += value;
    }
}
