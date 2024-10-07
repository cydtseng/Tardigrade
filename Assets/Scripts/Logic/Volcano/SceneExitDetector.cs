using UnityEngine;

public class SceneExitDetector : MonoBehaviour
{
    public bool hasSatisfiedLevel = false;
    public string nextScene;

    public void setHasSatisfiedLevel()
    {
        hasSatisfiedLevel = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && hasSatisfiedLevel)
        {
            Debug.Log("Player exited the scene on the right!");
            MusicSelection selection = GetComponent<MusicSelection>();
            if (selection) selection.gameObject.SetActive(true);
            Initiate.Fade(nextScene, Color.black, 1);
        }
    }
}