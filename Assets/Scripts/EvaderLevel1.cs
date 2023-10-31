using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using TMPro;
using Random = UnityEngine.Random;

public class EvaderLevel1 : MonoBehaviour
{
    // public static Text ShieldCount;
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


    public RawImage up;
    public RawImage right;
    public RawImage left;

    public GameObject JumpSpace;

    private float disableChaserTime = 17f;

    void Start()
    {
        JumpSpace.SetActive(false);
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
        if(LevelSelector.chosenLevel != 1){
            wormhole = GameObject.Find("wormhole");
            wormhole.gameObject.SetActive(false);
        }
        // ShieldCount.text = "x" + EvaderSpace.shieldCollected;

    }
    

    void Update()
    {
            
            float moveInput = Input.GetAxis("Horizontal");
            Vector2 moveDirection = new Vector2(moveInput, 0);
            if (rb.velocity.magnitude > 5f && !evaderMoved)
            {
                evaderMoved = true;
                StartRunning();
                StartCoroutine(RemoveRawImagesAfterDelay(2.0f));
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

    private IEnumerator RemoveRawImagesAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        up.gameObject.SetActive(false);
        right.gameObject.SetActive(false);
        left.gameObject.SetActive(false);
        yield return new WaitForSeconds(2.0f);
        JumpSpace.SetActive(true);
        StartCoroutine(ShakeObject(JumpSpace, 3f, 0.1f));
        
    }

    IEnumerator ShakeObject(GameObject obj, float duration, float intensity)
    {
        Vector3 originalPosition = obj.transform.position;
        Quaternion originalRotation = obj.transform.rotation;

        float endTime = Time.time + duration;

        while (Time.time < endTime)
        {
            float randomX = Random.Range(-intensity, intensity);
            float randomY = Random.Range(-intensity, intensity);

            obj.transform.position = originalPosition + new Vector3(randomX, randomY, 0);
            obj.transform.rotation = originalRotation * Quaternion.Euler(0, 0, Random.Range(-intensity, intensity));

            yield return null;
        }

        // Reset the object to its original position and rotation when the shake is done.
        obj.transform.position = originalPosition;
        obj.transform.rotation = originalRotation;
        JumpSpace.SetActive(false);
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
        if(collision.gameObject.CompareTag("Chaser"))
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
        StartCoroutine(sendDataScript.SendDataToGoogleSheets(survivalDuration.ToString(), Teleport.teleportUsed, "lost", (10-platformCount).ToString(), TimerScriptPractice.totalShieldsCollected));
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
        TimerScript.setTime();
    }

     public void NextButtonClicked()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        Debug.Log(currentScene.name);
        if(currentScene.name == "Level1"){
        SceneManager.LoadScene("Level2");
        }else{
            SceneManager.LoadScene("Level3");
        }
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
