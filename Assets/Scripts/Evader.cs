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
    public static int platformCount = 10;
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
    private GameObject countdown;
    private bool onSafeLedge = false;
    private CountdownScript countdownController;
    private bool hasCollidedWithChaser = false;
    private bool safeLedgeUsed = false;
    private GameObject SafeLedge;
    public Material ledgeMaterial;
    bool isCollidingWithLedge = false;
    Collision2D currentCollision;

    void Start()
    {
        evaderMoved = false;
        platformCount = 10;
        isColliding = false;
        onSafeLedge = false;
        safeLedgeUsed = false;
        hasCollidedWithChaser = false;
        HideGameOverShowTimer();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameObject chaser = GameObject.Find("Chaser");
        GameObject timer = GameObject.Find("TimerTxt");
        countdown = GameObject.Find("Countdown");
        SafeLedge = GameObject.Find("SafeLedge");
        countdown.SetActive(false);
        chaserSpriteRenderer = chaser.GetComponent<SpriteRenderer>();
        chaserController = chaser.GetComponent<ChaserAI>();
        timerController = timer.GetComponent<TimerScript>();
        countdownController = countdown.GetComponent<CountdownScript>();
        survivalStartTime = Time.time;
        if(EvaderSpace.shield) {
            Color greenColor = HexToColor("#6AF802");
            Vector3 newScale = new Vector3(5.0f, 5.0f, 5.0f);
            spriteRenderer.material.color = greenColor;
            GameObject evader = GameObject.Find("Evader");
            evader.transform.localScale = newScale;
        }
        if (EvaderSpace.shieldCollected == 0)
        {
            Color yellowColor = HexToColor("#FFFF00");
            Vector3 newScale = new Vector3(3.0f, 3.0f, 3.0f);
            spriteRenderer.material.color = yellowColor;
            GameObject evader = GameObject.Find("Evader");
            evader.transform.localScale = newScale;
        }
        if (LevelSelector.chosenLevel != 1){
            wormhole = GameObject.Find("wormhole");
            wormhole.gameObject.SetActive(false);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LedgePrefab"))
        {
            isCollidingWithLedge = true;
            currentCollision = collision;
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
        

        float gameplayDuration = Time.time - survivalStartTime;

        if(LevelSelector.chosenLevel != 1 && gameplayDuration > 10 && !EvaderSpace.visited){
            wormhole.gameObject.SetActive(true);
        }

        ShieldCount.text = "x" + EvaderSpace.shieldCollected;

        if (isCollidingWithLedge && Input.GetKeyDown(KeyCode.N))
        {
            Destroy(currentCollision.gameObject);
            platformCount++;
            LedgeCount.text = "x " + platformCount;
            isCollidingWithLedge = false;
        }
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
            else if(LevelSelector.chosenLevel != 1 && !hasCollidedWithChaser){
                hasCollidedWithChaser = true;
                if(EvaderSpace.shieldCollected == 0){
                    ShowGameOverHideTimer();
                    Debug.Log("set time called in collision");
                    TimerScript.setTime();
                }
                else{
                    EvaderSpace.shieldCollected -= 1;

                    Vector3 chaserPosition = collision.gameObject.transform.position;
                    collision.gameObject.transform.position = new Vector3(46.62f, 9.9f, 0);

                    if(EvaderSpace.shieldCollected == 0){
                        Color yellowColor = HexToColor("#FFFF00");
                        Vector3 newScale = new Vector3(3.0f, 3.0f, 3.0f);
                        spriteRenderer.material.color = yellowColor;
                        GameObject evader = GameObject.Find("Evader");
                        evader.transform.localScale = newScale;
                    }
                }
            }
        } else if(collision.gameObject.CompareTag("SafeLedge") && safeLedgeUsed == false && IsPlayerAboveLedge(collision.gameObject.transform.position.y)){
            onSafeLedge = true;
            safeLedgeUsed = true;
            chaserController.StopChasing();
            countdown.SetActive(true);
            countdownController.StartCountdown(5f);
            StartCoroutine(ResumeChasingAfterDelay(7f));
        }else{
            isGrounded = true;
        }
    }

     private bool IsPlayerAboveLedge(float ledgeY)
    {
        float playerY = transform.position.y;

        Debug.Log("Ledge position:" + ledgeY);
        Debug.Log("Player position:"+ playerY);

        // Adjust the following value based on your game's logic to define "above."
        float verticalThreshold = 2.0f;

        return playerY > ledgeY + verticalThreshold;
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
        Renderer renderer = SafeLedge.GetComponent<Renderer>();
        renderer.material = ledgeMaterial;
        onSafeLedge = false;
        chaserController.StartChasing();
        countdown.SetActive(false);
    }
}

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
        StartCoroutine(sendDataScript.SendDataToGoogleSheets(survivalDuration.ToString(), Teleport.teleportUsed, "lost", (10-platformCount).ToString(), EvaderSpace.totalShieldsCollected.ToString()));
    }

    void HideGameOverShowTimer()
    {
        GameText.gameObject.SetActive(false);
        TimerTxt.gameObject.SetActive(true);
        RestartText.gameObject.SetActive(false);
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
        EvaderSpace.shieldCollected = 0;
        EvaderSpace.visited = false;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

     public void NextButtonClicked()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name == "Level1"){
            LevelSelector.chosenLevel = 2;
            SceneManager.LoadScene("Level2");
        }else{
            LevelSelector.chosenLevel = 3;
            SceneManager.LoadScene("Level3");
        }
    }

     public void BackButtonClicked()
    {
        Time.timeScale = 1f;
        EvaderSpace.shieldCollected = 0;
        EvaderSpace.visited = false;
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
