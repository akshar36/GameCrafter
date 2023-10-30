using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using TMPro;
public class EvaderLevel1 : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpForce = 8.0f;
    private Rigidbody2D rb;
    private bool isGrounded;
    private ChaserAI chaserController;
    private TimerScript timerController;
    private int platformCount = 10;
    public GameObject floorprefab;
    private GameObject chaser;
    public Text GameText;
    public GameObject RestartText;
    public GameObject BackButton;
    public Text LedgeCount;
    public Text TimerTxt;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer chaserSpriteRenderer;
    public Sprite caughtSprite;
    public Sprite smilingSprite;
    //public Sprite shieldSprite;
    public SendData sendDataScript;

    public Sprite hitOne;
    public Sprite hitTwo;
    private int getHit = 0;

    private bool evaderMoved = false;
    public static float survivalStartTime;
    private bool noShield = true;
    private GameObject DroppedLedge;
    private bool isColliding = false;
    private GameObject LedgePrefab;
    private GameObject wormhole;

    private float disableChaserTime = 12f;

    void Start()
    {
        HideGameOverShowTimer();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameObject chaser = GameObject.Find("Chaser");
        GameObject timer = GameObject.Find("TimerTxt");
        chaserSpriteRenderer = chaser.GetComponent<SpriteRenderer>();
        chaserController = chaser.GetComponent<ChaserAI>();
        timerController = timer.GetComponent<TimerScript>();
        survivalStartTime = Time.time;
        if(EvaderSpace.shield) {
            StartCoroutine(DeactivateShield(10f));
            noShield = false;            
            Color greenColor = HexToColor("#6AF802");
            Vector3 newScale = new Vector3(5.0f, 5.0f, 5.0f);
            spriteRenderer.material.color = greenColor;
            GameObject evader = GameObject.Find("Evader");
            evader.transform.localScale = newScale;
        }
        if(LevelSelector.chosenLevel == 2){
            wormhole = GameObject.Find("wormhole");
            wormhole.gameObject.SetActive(false);
        }

    }
    

    void Update()
    {
            
            float moveInput = Input.GetAxis("Horizontal");
            Vector2 moveDirection = new Vector2(moveInput, 0);
            if (rb.velocity.magnitude > 5f && !evaderMoved)
            {
                Debug.Log("rb.velocity.magnitude " + rb.velocity.magnitude);
                evaderMoved = true;
                StartRunning();
                StartCoroutine(EnableChaserAfterDelay(disableChaserTime));
            }

            rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);

            if (isGrounded && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
            {
                Jump();
            }

            // if (Input.GetKeyDown(KeyCode.M))
            // {
            //     if (DroppedLedge != null)
            //     {
            //         DroppedLedge.transform.Rotate(Vector3.forward * 90.0f);
            //     }
            // }

            if (!isGrounded && Input.GetKeyDown(KeyCode.Space) && platformCount > 0)
            {
                DroppedLedge = Instantiate(floorprefab, transform.position, Quaternion.identity);
                platformCount--;
                LedgeCount.text = "x " + platformCount;
                LedgeCount.gameObject.SetActive(true);
                isGrounded = true;
            }
        

        if(LevelSelector.chosenLevel == 2 && Time.time > 10 && !EvaderSpace.visited){
            wormhole.gameObject.SetActive(true);
        }
    }

    private IEnumerator EnableChaserAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(RedReminderFlash());
        yield return new WaitForSeconds(3.0f);
        chaserSpriteRenderer.color = Color.yellow;
        chaserController.StartChasing();
    }
    private IEnumerator RedReminderFlash()
    {
        // Flash the chaser red to remind the player
        Color originalColor = chaserSpriteRenderer.color;
        for (int i = 0; i < 3; i++) // Flash 3 times
        {
            chaserSpriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.5f); // Flash duration
            chaserSpriteRenderer.color = originalColor;
            yield return new WaitForSeconds(0.5f); // Time between flashes
        }
    }


    private IEnumerator DeactivateShield(float delay)
    {
        yield return new WaitForSeconds(delay);
        Color greenColor = HexToColor("#FFFF00");
        Vector3 newScale = new Vector3(3.0f, 3.0f, 3.0f);
        spriteRenderer.material.color = greenColor;
        GameObject evader = GameObject.Find("Evader");
        evader.transform.localScale = newScale;
        noShield = true;
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if((LevelSelector.chosenLevel == 1 && collision.gameObject.CompareTag("Chaser")) || 
        (LevelSelector.chosenLevel == 2 && collision.gameObject.CompareTag("Chaser") && noShield))
        {
            getHit++;
            if (getHit == 1)
            {
                spriteRenderer.sprite = hitOne;
                transform.position = new Vector3(46.62f, 9.9f, 0);
            }
            else if (getHit == 2)
            {
                spriteRenderer.sprite = hitTwo;
                transform.position = new Vector3(46.62f, 9.9f, 0);
            }
            else if (getHit == 3)
            {
                ShowGameOverHideTimer();
                Debug.Log("set time called in collision");
                TimerScript.setTime();
            }
        } else{
            isGrounded = true;
        }

    }

// void OnCollisionStay2D(Collision2D collision)
//      {
//          if (collision.gameObject.CompareTag("LedgePrefab"))
//          {
//              if(Input.GetKeyDown(KeyCode.N)){
//                  Destroy(collision.gameObject);
//                  platformCount++;
//                  LedgeCount.text = "x " + platformCount;
//              }
//          }
//      }


    public void ShowGameOverHideTimer()
    {
        GameText.text = "GAME OVER";
        GameText.gameObject.SetActive(true);
        TimerTxt.gameObject.SetActive(false);
        chaserSpriteRenderer.sprite = smilingSprite;
        spriteRenderer.sprite = caughtSprite;
        RestartText.gameObject.SetActive(true);
        Time.timeScale = 0f;
        float survivalDuration = Time.time - survivalStartTime;
        StartCoroutine(sendDataScript.SendDataToGoogleSheets(survivalDuration.ToString(), Teleport.teleportUsageCount.ToString(), "lost"));
    }

    void HideGameOverShowTimer()
    {
        GameText.gameObject.SetActive(false);
        TimerTxt.gameObject.SetActive(true);
        RestartText.gameObject.SetActive(false);
    }

    public void StartRunning()
    { 
        timerController.StartTime();
    }

    public void RestartButtonClicked()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

     public void BackButtonClicked()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("LevelSelection");
        Debug.Log("Back clicked");
        Debug.Log("set time called on back");
        TimerScript.setTime();
    }

    Color HexToColor(string hex)
    {
        // Remove the '#' character if it's included
        hex = hex.TrimStart('#');

        // Parse the hex string to a Color object
        Color color = new Color();
        ColorUtility.TryParseHtmlString("#" + hex, out color);

        return color;
    }
}
