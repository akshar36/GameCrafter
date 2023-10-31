using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EvaderSpace : MonoBehaviour
{
    
    public Text GameOverTxt;
    private TimerScriptPractice timerController;
    public Text TimerTxt;
    public static int shieldCollected = 0;   
    public static int totalShieldsCollected = 0; 
    public Text shieldCount;
    private int bombHit = 0;    
    public float moveForce = 10.0f;
    private Rigidbody2D rb;
    public GameObject RestartText;
    // private float endX = -2.50f;
    private SpriteRenderer spriteRenderer;
    public Sprite hitOne;
    public Sprite hitTwo;
    public Sprite hitThree;
    public static bool shield = false;
    public static bool visited = false;
    void Start()
    {
        visited = false;
        shield = false;
        shieldCollected = 0;
        totalShieldsCollected = 0;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameOverTxt.gameObject.SetActive(false);
        GameObject timer = GameObject.Find("TimerTxt");
        timerController = timer.GetComponent<TimerScriptPractice>();
        timerController.StartTime();
        RestartText.gameObject.SetActive(false);
        visited = true;
    }

    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        // float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(0, verticalInput * moveForce);
            rb.velocity = movement;
    }

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "shield")
        {
            shieldCollected++;
            totalShieldsCollected++;
            shieldCount.text = shieldCollected.ToString();
            collision.gameObject.SetActive(false);

            if(shieldCollected > 0) {
                shield = true;
            }
        }

        if(collision.tag == "bomb")
        {
            bombHit ++;
            collision.gameObject.SetActive(false);
            if(bombHit == 1){
                spriteRenderer.sprite = hitOne;
            } else if(bombHit == 2){
                spriteRenderer.sprite = hitTwo;
            } else if(bombHit == 3){
                spriteRenderer.sprite = hitThree;
                if(LevelSelector.chosenLevel == 2)
                    SceneManager.LoadScene("Level2");
                else if(LevelSelector.chosenLevel == 3)
                    SceneManager.LoadScene("Level3");
            }
        }
    }

}
