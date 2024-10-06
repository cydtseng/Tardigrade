using System.Collections;
using UnityEngine;


public class ImmediateTransition : MonoBehaviour
{
    public string nextSceneName;  // Set the next scene's name in the Unity Inspector

    void Start()
    {
        // StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        Initiate.Fade(nextSceneName, Color.black, 1);
        yield break;
    }

    public void onClick()
    {
        Initiate.Fade(nextSceneName, Color.black, 1);
        // SceneManager.LoadScene("Blank");
    }

    public void quitOnClick() {
        Application.Quit();
    }
}