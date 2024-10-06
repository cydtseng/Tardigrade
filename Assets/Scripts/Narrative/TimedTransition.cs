using System.Collections;
using UnityEngine;


public class TimedTransition : MonoBehaviour
{
    public string nextSceneName;  // Set the next scene's name in the Unity Inspector

    void Start()
    {
        StartCoroutine(WaitAndLoadScene());
    }

    IEnumerator WaitAndLoadScene()
    {
        yield return new WaitForSeconds(5);  // Wait for 5 seconds
        Initiate.Fade(nextSceneName, Color.black, 1);
    }
}