using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using TMPro;
public class Evader : MonoBehaviour
{
    public Text ShieldCount;
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
    public Sprite shieldSprite;
    private bool evaderMoved = false;
    public SendData sendDataScript;
    public static float survivalStartTime;
    private GameObject DroppedLedge;
    private bool isColliding = false;
    private GameObject LedgePrefab;
    private GameObject wormhole;
    private bool onSafeLedge = false;
    private CountDownScript countdownController;
    public Text CountDownText;
    private bool hasCollidedWithChaser = false;

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
                evaderMoved = true;
                StartRunning();
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
        ShieldCount.text = "x" + EvaderSpace.shieldCollected;
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Chaser"))
        {
            if(LevelSelector.chosenLevel == 1){
                ShowGameOverHideTimer();
                Debug.Log("set time called in collision");
                TimerScript.setTime();
            }
            else if(LevelSelector.chosenLevel == 2 && !hasCollidedWithChaser){
                hasCollidedWithChaser = true;
                if(EvaderSpace.shieldCollected == 0){
                    ShowGameOverHideTimer();
                    Debug.Log("set time called in collision");
                    TimerScript.setTime();
                }
                else{
                    EvaderSpace.shieldCollected -= 1;   
                    if(EvaderSpace.shieldCollected == 0){
                        Color yellowColor = HexToColor("#FFFF00");
                        Vector3 newScale = new Vector3(3.0f, 3.0f, 3.0f);
                        spriteRenderer.material.color = yellowColor;
                        GameObject evader = GameObject.Find("Evader");
                        evader.transform.localScale = newScale;
                    }
                }
            }
            
        } else if(collision.gameObject.CompareTag("SafeLedge")){
            Debug.Log("ON SAFE LEDGE");
            onSafeLedge = true;
            chaserController.StopChasing();
            CountDownText.gameObject.SetActive(true);
            StartCoroutine(ResumeChasingAfterDelay(5f));
        }else{
            isGrounded = true;
        }

    }

    void OnCollisionExit2D(Collision2D collision) {
        if (hasCollidedWithChaser && collision.gameObject.CompareTag("Chaser")) {
            hasCollidedWithChaser = false;
        }
    }

    private IEnumerator ResumeChasingAfterDelay(float delay)
    {
    yield return new WaitForSeconds(delay);
    if (onSafeLedge)
    {
        onSafeLedge = false;
        chaserController.StartChasing();
        Debug.Log("resuming chasing");
        CountDownText.gameObject.SetActive(false);
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
        CountDownText.gameObject.SetActive(false);
    }

    public void StartRunning()
    {
        chaserController.StartChasing();
        timerController.StartTime();
    }

    public void RestartButtonClicked()
    {
        Time.timeScale = 1f;
        hasCollidedWithChaser = false;
        EvaderSpace.shield = false;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

     public void NextButtonClicked()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name == "Level1"){
        SceneManager.LoadScene("Level2");
        }else{
            SceneManager.LoadScene("Level3");
        }
    }

     public void BackButtonClicked()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("LevelSelection");
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
