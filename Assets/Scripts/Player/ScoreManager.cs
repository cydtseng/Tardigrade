using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public Dictionary<string, float> playRecord;  // with default of 0

    void Start() {
        Reset();
    }

    // dictionary access methods for convenience + abstraction
    public float Get(string key) {
        if (!playRecord.ContainsKey(key)) {
            playRecord.Add(key, 0f);
            return 0f;
        }
        return playRecord[key];
    }
    public void Set(string key, float value) {
        playRecord[key] = value;
    }
    public void SetRelative(string key, float value) {
        Set(key, Get(key) + value);
    }
    public void SetIncrement(string key) {
        SetRelative(key, 1f);
    }
    public void Reset() {
        playRecord = new Dictionary<string, float>();
    }
}
