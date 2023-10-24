using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EvaderSpace : MonoBehaviour
{
    public TextMeshPro ShieldCount;
    public TextMeshPro GameOverTxt;
    public TextMeshPro TimerTxt;
    private TimerScript timerController;
    private int shieldCollected = 0;    
    private int bombHit = 0;    
    public float moveForce = 10.0f;
    private Rigidbody2D rb;
    private float endX = -2.50f;
    private SpriteRenderer spriteRenderer;
    public Sprite hitOne;
    public Sprite hitTwo;
    public Sprite hitThree;
    public static bool shield = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameOverTxt.gameObject.SetActive(false);
        GameObject timer = GameObject.Find("TimerTxt");
        timerController = timer.GetComponent<TimerScript>();
    }

    void Update()
    {
        timerController.StartTimeAtL2();
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(horizontalInput * moveForce, verticalInput * moveForce);
        if (transform.position.x < endX)
        {
            rb.velocity = movement;
        }
        else
        {
            transform.position = new Vector2(transform.position.x-1.5f , transform.position.y );
        }
    }

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "shield")
        {
            if(shieldCollected < 10) {
                shieldCollected ++;
                ShieldCount.text = shieldCollected + " / 10";
                ShieldCount.gameObject.SetActive(true);
                collision.gameObject.SetActive(false);
            }
            if(shieldCollected >1) {
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
                Time.timeScale = 0f;
            }
        }
    }
}
