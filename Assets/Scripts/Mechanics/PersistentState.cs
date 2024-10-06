using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PersistentState : MonoBehaviour
{
    private int scoreCounter = 0;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("PersistentStateSingleton");
        if (objs.Length > 1) Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
    }
 
    void Start()
    {
        
    }
 
    // Update is called once per frame
    void Update()
    {
        
    }

    public void incrementScore() {
        scoreCounter += 1;
    }

    public int getScore() {
        return scoreCounter;
    }

    public void addToScore(int value) {
        scoreCounter += value;
    }
}
