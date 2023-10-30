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
        if (collision.tag == "Player")
        {
            teleportUsageCount++;
            // Instead of teleporting within the scene, load the tutorial scene
            player.transform.position = new Vector2(portal.transform.position.x + 7f, portal.transform.position.y);
            SceneManager.LoadScene("Space");
            
        }
    }
}
