using UnityEngine;

public class SceneExitDetector : MonoBehaviour
{
    private bool hasSatisfiedLevel = false;
    
    public void setHasSatisfiedLevel()
    {
        hasSatisfiedLevel = true;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && hasSatisfiedLevel)
        {
            Debug.Log("Player exited the scene on the right!");
            Initiate.Fade("300MillionYears", Color.black, 1);
        }
    }
}