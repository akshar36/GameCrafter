using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TutorialManagerScript : MonoBehaviour
{
    private float timeInTutorial = 10f; // Time to spend in the tutorial
    private string sceneToReturnTo = "SampleScene"; // The name of the scene you want to return to

    void Start()
    {
        StartCoroutine(ReturnToSceneAfterDelay(timeInTutorial));
    }

    private IEnumerator ReturnToSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneToReturnTo);
    }
}