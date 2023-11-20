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
    public Text ShieldCount;
    private int shields = 2;
    public float moveSpeed = 5.0f;
    public float jumpForce = 11.0f;
    public float jumpForce1 = 8.0f;
    public float jumpForce2 = 11.0f;
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
    public GameObject Recollect;
    public GameObject shiftKey;
    public GameObject smoke;
    public static int portalCount = 5;
    private Vector2[] positions = new Vector2[]
    {
        new Vector2(105.4f, 1f),
        new Vector2(30f, 1f),
        new Vector2(31f, 46f),
        new Vector2(89f, 45f)
    };
    private Vector3 upOffset;
    private Vector3 rightOffset;
    private Vector3 leftOffset;
    private bool shiftNotclicked = true;
    private float disableChaserTime = 17f;
    bool isCollidingWithLedge = false;
    Collision2D currentCollision;
    private bool isNkeyShown = false;
    Vector2? deathPosition = null;
    private GameObject collectTeleport;
    private Vector2 respawnPosition = new Vector2(31f, 45f);
    private bool hasCollidedWithChaser = false;

    public GameObject shieldHighlight;
    public GameObject portalHighlight;

    void Start()
    {
        portalHighlight.SetActive(false);
        shieldHighlight.SetActive(false);
        shields = 2;
        portalCount = 5;
        hasCollidedWithChaser = false;
        JumpSpace.SetActive(false);
        Recollect.SetActive(false);
        isNkeyShown = false;
        HideGameOverShowTimer();
        platformCount = 10;
        rb = GetComponent<Rigidbody2D>();
        shiftKey.SetActive(false);
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameObject chaser = GameObject.Find("Chaser");
        GameObject timer = GameObject.Find("TimerTxt");
        collectTeleport = GameObject.Find("add_teleport");
        collectTeleport.SetActive(true);
        chaserSpriteRenderer = chaser.GetComponent<SpriteRenderer>();
        chaserController = chaser.GetComponent<ChaserAI>();
        timerController = timer.GetComponent<TimerScript>();
        survivalStartTime = Time.time;
        isCollidingWithLedge = false;
        deathPosition = null;
        //if(EvaderSpace.shield) {
        //    StartCoroutine(DeactivateShield(10f));
        //    noShield = false;            
        //    Color greenColor = HexToColor("#6AF802");
        //    Vector3 newScale = new Vector3(5.0f, 5.0f, 5.0f);
        //    spriteRenderer.material.color = greenColor;
        //    GameObject evader = GameObject.Find("Evader");
        //    evader.transform.localScale = newScale;
        //}
        Color yellowColor = HexToColor("#FFFF00");
        Vector3 newScale = new Vector3(3.0f, 3.0f, 3.0f);
        spriteRenderer.material.color = yellowColor;
        GameObject evader = GameObject.Find("Evader");
        evader.transform.localScale = newScale;
        //if (LevelSelector.chosenLevel != 1){
        //    wormhole = GameObject.Find("wormhole");
        //    wormhole.gameObject.SetActive(false);
        //}
        // ShieldCount.text = "x" + EvaderSpace.shieldCollected;
        upOffset = up.transform.position - transform.position;
        rightOffset = right.transform.position - transform.position;
        leftOffset = left.transform.position - transform.position;

    }
    

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LedgePrefab"))
        {
            isCollidingWithLedge = true;
            currentCollision = collision;
        }
        else
        {
            isCollidingWithLedge = false;
            currentCollision = null;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LedgePrefab"))
        {
            isCollidingWithLedge = false;
            currentCollision = null;
        }
    }

    void Update()
    {
            up.transform.position = transform.position + upOffset;
            right.transform.position = transform.position + rightOffset;
            left.transform.position = transform.position + leftOffset;

            float moveInput = Input.GetAxis("Horizontal");
            if(moveInput > 0){
                right.gameObject.SetActive(false);
            }
            else if(moveInput < 0){
                left.gameObject.SetActive(false);
            }
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
                up.gameObject.SetActive(false);
                Jump();
            }

            // if (Input.GetKeyDown(KeyCode.M))
            // {
            //     if (DroppedLedge != null)
            //     {
            //         DroppedLedge.transform.Rotate(Vector3.forward * 90.0f);
            //     }
            // }

            if (portalCount >0 && ((Input.GetKeyDown(KeyCode.LeftShift)) || (Input.GetKeyDown(KeyCode.RightShift)))){
                shiftKey.SetActive(false);
                Debug.Log("playing");
                GameObject particleInstance = Instantiate(smoke, transform.position, Quaternion.identity);
                particleInstance.GetComponent<ParticleSystem>().Play();
                portalCount --;
                Text portalCountText = GameObject.Find("PortalCount").GetComponent<Text>();
                portalCountText.text = "x"+portalCount;
                Vector3 chaserPosition = this.transform.position; //69 24
                Debug.Log("aaa");
                if (chaserPosition.x <= 69 && chaserPosition.y <= 20)
                {
                 MoveToRandomPosition(new Vector2(110f, 45f));
                }
                if (chaserPosition.x <= 69 && chaserPosition.y >= 20)
                {
                 MoveToRandomPosition(new Vector2(110f, 1f));
                 }
                if (chaserPosition.x >= 69 && chaserPosition.y <= 20)
                {
                    MoveToRandomPosition(new Vector2(30f, 45f));
                }
                if (chaserPosition.x >= 69 && chaserPosition.y >= 20)
                {
                    MoveToRandomPosition(new Vector2(30f, 1f));
                }
            }

            if (!isGrounded && Input.GetKeyDown(KeyCode.Space) && platformCount > 0)
            {
                JumpSpace.SetActive(false);
                if(!isNkeyShown){
                    Recollect.SetActive(true);
                    isNkeyShown = true;
                }
                DroppedLedge = Instantiate(floorprefab, transform.position, Quaternion.identity);
                platformCount--;
                LedgeCount.text = "x " + platformCount;
                LedgeCount.gameObject.SetActive(true);
                isGrounded = true;
                if (shiftNotclicked)
                    StartCoroutine(removeShift(7.0f));
            }
        

        // if(LevelSelector.chosenLevel == 2 && Time.time > 10 && !EvaderSpace.visited){
        //     wormhole.gameObject.SetActive(true);
        // }

        if (isCollidingWithLedge && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            Recollect.SetActive(false);
            Destroy(currentCollision.gameObject);
            platformCount++;
            LedgeCount.text = "x " + platformCount;
            isCollidingWithLedge = false;
            currentCollision = null;  
        }
    }

    private IEnumerator RemoveRawImagesAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        yield return new WaitForSeconds(2.0f);
        JumpSpace.SetActive(true);
        StartCoroutine(ShakeObject(JumpSpace, 3f, 0.1f));
        
    }

    private IEnumerator removeShift(float delay)
    {
        yield return new WaitForSeconds(delay);
        shiftKey.SetActive(true);
        shiftNotclicked = false;
    }

    private void MoveToRandomPosition(Vector2 target)
    {
        Vector2 targetPosition = target;
        GameObject particleInstance = Instantiate(smoke, targetPosition, Quaternion.identity);
        Debug.Log(targetPosition.x);
        Debug.Log(targetPosition.y);
        particleInstance.GetComponent<ParticleSystem>().Play();
        transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
    }

    private IEnumerator ShakeObject(GameObject obj, float duration, float intensity)
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
        // rb.velocity = new Vector2(jumpVelocity, jumpForce);
        // Scene currentScene = SceneManager.GetActiveScene();
        // if(currentScene.name == "Level1"){
        //      rb.AddForce(Vector2.up * jumpForce1, ForceMode2D.Impulse);
        // }else{
        //      rb.AddForce(Vector2.up * jumpForce2, ForceMode2D.Impulse);
        // }
        isGrounded = false;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if(!hasCollidedWithChaser && collision.gameObject.CompareTag("Chaser"))
        {
            deathPosition = collision.transform.position;
            getHit++;
            hasCollidedWithChaser = true;

            if(shields > 0){
                shields--;
                ShieldCount.text = "x" + shields.ToString();
            }

            if (getHit == 3)
            {
                ShowGameOverHideTimer();
                TimerScript.setTime();
            }
            else{
                StartCoroutine(ShieldHighlightFlash());
                StartCoroutine(RespawnPlayer());
            }
            
        } 
        else if(collision.gameObject.CompareTag("AddTeleport"))
        {
            collectTeleport.SetActive(false);
            portalCount += 5;
            Text portalCountText = GameObject.Find("PortalCount").GetComponent<Text>();
            portalCountText.text = "x" + portalCount;
            StartCoroutine(PortalHighlightFlash());
        }
        else{
            isGrounded = true;
        }

    }

    IEnumerator RespawnPlayer()
    {
        spriteRenderer.enabled = false;
        
        yield return new WaitForSeconds(1f);

        hasCollidedWithChaser = false;
        transform.position = respawnPosition;
        chaserSpriteRenderer.transform.position = new Vector2(90f, 20f);
        spriteRenderer.enabled = true;
    }

    private IEnumerator PortalHighlightFlash()
    {
        portalHighlight.SetActive(true);
        SpriteRenderer portalHighlightRenderer = portalHighlight.GetComponent<SpriteRenderer>();
        // Flash the chaser red to remind the player
        Color originalColor = portalHighlightRenderer.color;
        for (int i = 0; i < 3; i++) // Flash 3 times
        {
            portalHighlightRenderer.color = Color.red;
            yield return new WaitForSeconds(0.5f); // Flash duration
            portalHighlightRenderer.color = originalColor;
            yield return new WaitForSeconds(0.5f); // Time between flashes
        }
        portalHighlight.SetActive(false);
    }

    private IEnumerator ShieldHighlightFlash()
    {
        shieldHighlight.SetActive(true);
        SpriteRenderer shieldHighlightRenderer = shieldHighlight.GetComponent<SpriteRenderer>();
        // Flash the chaser red to remind the player
        Color originalColor = shieldHighlightRenderer.color;
        for (int i = 0; i < 3; i++) // Flash 3 times
        {
            shieldHighlightRenderer.color = Color.red;
            yield return new WaitForSeconds(0.5f); // Flash duration
            shieldHighlightRenderer.color = originalColor;
            yield return new WaitForSeconds(0.5f); // Time between flashes
        }
        shieldHighlight.SetActive(false);
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
        StartCoroutine(sendDataScript.SendDataToGoogleSheets(survivalDuration.ToString(), Teleport.wormholeUsed, (5-portalCount).ToString(), "chaser", 
        (10-platformCount).ToString(), "0", EvaderSpace.totalShieldsCollected.ToString(), ChaserAI.timesStuck.ToString(), deathPosition.ToString()));
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
            LevelSelector.chosenLevel = 2;
            SceneManager.LoadScene("Level2");
        }else{
            LevelSelector.chosenLevel = 3;
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
