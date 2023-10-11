using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject portal;
    private GameObject player;
    private GameObject chaser;

   
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        chaser = GameObject.FindWithTag("Chaser");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            player.transform.position = new Vector2(portal.transform.position.x, portal.transform.position.y);
        } else if(collision.tag == "Chaser"){
            chaser.transform.position = new Vector2(portal.transform.position.x, portal.transform.position.y);
        }
    }
}
