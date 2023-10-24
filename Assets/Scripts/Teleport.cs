using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    public GameObject portal;
    private GameObject player;
    private GameObject chaser;
    public static int teleportUsageCount = 0;
    public SendData sendDataScript;
    public string tutorialSceneName = "Tutorial1";
    public string fromSceneName;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        chaser = GameObject.FindWithTag("Chaser");
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if(collision.tag == "Player")
    //     {
    //         player.transform.position = new Vector2(portal.transform.position.x, portal.transform.position.y);
    //         teleportUsageCount++;
    //         Debug.Log("Teleport usage incremented to: " + teleportUsageCount);
    //     } 
    //     else if(collision.tag == "Chaser"){
    //         chaser.transform.position = new Vector2(portal.transform.position.x, portal.transform.position.y);
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && portal.name =="wormhole")
        {
            // Instead of teleporting within the scene, load the tutorial scene
            SceneManager.LoadScene(tutorialSceneName);

            // After 10 seconds, come back to the original scene.
            // The StartCoroutine method allows you to use a coroutine for this.
            StartCoroutine(ReturnToOriginalSceneAfterDelay(10f));
            // TimerScriptPractice.mainSceneTimeLeft = TimerScript.TimeLeft;
        }
        else if (collision.tag == "Player")
        {
            player.transform.position = new Vector2(portal.transform.position.x + 7f, portal.transform.position.y);
        } else {
            chaser.transform.position = new Vector2(portal.transform.position.x +7f, portal.transform.position.y);
        }
    }

    private IEnumerator ReturnToOriginalSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // This will load the original scene based on the scene's build index.
        // Here we're assuming the original scene is at build index 0.
        // You might want to change this if your original scene has a different index.
        SceneManager.LoadScene(fromSceneName);
    }
}
