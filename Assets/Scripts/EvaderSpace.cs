using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EvaderSpace : MonoBehaviour
{
    public Text ShieldCount;
    public Text GameOverTxt;
    private TimerScriptPractice timerController;
    public Text TimerTxt;
    private int shieldCollected = 0;    
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
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(horizontalInput * moveForce, verticalInput * moveForce);
            rb.velocity = movement;
    }

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "shield")
        {
            if(shieldCollected < 5) {
                shieldCollected ++;
                ShieldCount.text = shieldCollected + " / 5";
                ShieldCount.gameObject.SetActive(true);
                collision.gameObject.SetActive(false);
            }
            if(shieldCollected >= 5) {
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
                GameOverTxt.gameObject.SetActive(true);
                TimerTxt.gameObject.SetActive(false);
                RestartText.gameObject.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }

    public void RestartButtonClicked()
    {
        Time.timeScale = 1f;
        // Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("SampleScene2");
    }

     public void BackButtonClicked()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("LevelSelection");
    }

}