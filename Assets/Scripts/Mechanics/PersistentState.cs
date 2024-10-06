using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PersistentState : MonoBehaviour
{
    void Awake()
    {
        // GameObject[] objs = GameObject.FindGameObjectsWithTag("music");
        // if (objs.Length > 1)
        // {
        //     Destroy(this.gameObject);
        // }
        DontDestroyOnLoad(this.gameObject);
    }
 
    void Start()
    {
        
    }
 
    // Update is called once per frame
    void Update()
    {
        
    }
 
    public void Reset()
    {
        
    }

}
