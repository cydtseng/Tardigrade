using UnityEngine;

public class CountdownToNuclear : MonoBehaviour
{
    public float countdownTime = 20.0f; 
    private float elapsedTime = 0.0f;   
    public string nextScene;
    
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= countdownTime)
        {
           Initiate.Fade(nextScene, Color.black, 1);
        }
    }
}