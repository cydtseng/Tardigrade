using System.Collections;
using UnityEngine;


public class TimedTransition : MonoBehaviour
{
    public string nextSceneName;  // Set the next scene's name in the Unity Inspector
    public float waitDuration = 5f;

    void Start()
    {
        StartCoroutine(WaitAndLoadScene());
    }

    IEnumerator WaitAndLoadScene()
    {
        yield return new WaitForSeconds(waitDuration);  // Wait for 5 seconds
        Initiate.Fade(nextSceneName, Color.black, 1);
    }
}