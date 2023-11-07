using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BombScript : MonoBehaviour {
    private float speed = 10f;
    private Rigidbody2D rb;
    private Vector2 screenBounds;
    private int startTime = 100;

    void Start () {
        rb = this.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(-speed, 0);
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    void Update () {
        if(screenBounds.x *-3  > transform.position.x){
            if(this.gameObject != null)
                Destroy(this.gameObject);
        }
    }

    void removeBomb(){
        float TimeLeft = 10;
        if(startTime > 80)
        {
            if(TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
            }
            else
            {
                TimeLeft = 0;
                int totalShieldsCollected = 0;
                if(LevelSelector.chosenLevel == 1){
                    SceneManager.LoadScene("Scene1");
                    } 
                else if(LevelSelector.chosenLevel == 2){
                    SceneManager.LoadScene("Scene2");
                }
                else if(LevelSelector.chosenLevel == 3){
                    SceneManager.LoadScene("Scene3");
                }
            }
        }
    }
    
}