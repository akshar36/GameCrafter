using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using TMPro;
using Pathfinding;

public class Evader : MonoBehaviour
{
    public Text ShieldCount;
    public float moveSpeed = 5.0f;
    public float jumpForce = 11.0f;
    public float jumpForce1 = 8.0f;
    public float jumpForce2 = 11.0f;
    public float jumpVelocity = 1.0f;
    private Rigidbody2D rb;
    private bool isGrounded;
    private ChaserAI chaserController;
    private TimerScript timerController;
    public static int platformCount = 5;
    public static int totalPlatformCount = 5;
    public static int icePlatformCount = 0;
    public static int totalIcePlatformCount = 0;
    public GameObject floorprefab;
    public GameObject iceFloorPrefab;
    private GameObject chaser;
    public Text GameText;
    public GameObject RestartText;
    public GameObject BackButton;
    public Text LedgeCount;
    public Text iceLedgeCount;
    public Text TimerTxt;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer chaserSpriteRenderer;
    public Sprite caughtSprite;
    public Sprite smilingSprite;
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
    public Sprite powerEvader;
    public Sprite normalEvader;
    private int laserHit = 0;
    private GameObject hint;
    private GameObject collectTeleport;
    public static bool iceCollected = false;
    private bool normalLedgeSelected = true;
    private bool iceLedgeSelected = true;
    private bool ghostNotCalled = true;
    public GameObject ghostChaser;
    public GameObject smoke;
    public GameObject shiftKey;
    private bool shiftKeyNotPressed = true;
    public static int portalCount;
    public static int totalPortalCount;
    private Vector2[] positions = new Vector2[]
    {
        new Vector2(105.4f, 1f),
        new Vector2(30f, 1f),
        new Vector2(31f, 46f),
        new Vector2(89f, 45f)
    };


    public static string lostReason = "chaser";
    public static Vector2? deathPosition = null;
    public GameObject iceHighlight;
    public GameObject normalLedgeSprite;
    public GameObject shieldHighlight;
    public GameObject portalHighlight;

    void Start()
    {
        portalHighlight.SetActive(false);
        shieldHighlight.SetActive(false);
        iceHighlight.SetActive(false);
        evaderMoved = false;
        platformCount = 5;
        totalPlatformCount = 5;
        portalCount = 5;
        totalPortalCount = 5;
        icePlatformCount = 0;
        totalIcePlatformCount = 0;
        isColliding = false;
        onSafeLedge = false;
        safeLedgeUsed = false;
        hasCollidedWithChaser = false;
        isCollidingWithLedge = false;
        deathPosition = null;

        HideGameOverShowTimer();
        shiftKey.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        GameObject evader = GameObject.Find("Evader");
        spriteRenderer = evader.GetComponent<SpriteRenderer>();
        GameObject chaser = GameObject.Find("Chaser");
        GameObject timer = GameObject.Find("TimerTxt");
        hint = GameObject.Find("Hint");
        collectTeleport = GameObject.Find("add_teleport");
        collectTeleport.SetActive(true);
        countdown = GameObject.Find("Countdown");
        SafeLedge = GameObject.Find("SafeLedge");
        countdown.SetActive(false);
        if (hint != null)
            hint.SetActive(false);
        if (ghostChaser != null)
            ghostChaser.SetActive(false);
        chaserSpriteRenderer = chaser.GetComponent<SpriteRenderer>();
        chaserController = chaser.GetComponent<ChaserAI>();
        timerController = timer.GetComponent<TimerScript>();
        countdownController = countdown.GetComponent<CountdownScript>();
        survivalStartTime = Time.time;

        if (EvaderSpace.shield)
        {
            spriteRenderer.sprite = powerEvader;
            Color greenColor = HexToColor("#6AF802");
            Vector3 newScale = new Vector3(5.0f, 5.0f, 5.0f);
            spriteRenderer.material.color = greenColor;
            spriteRenderer.transform.localScale = newScale;
        }
        if (EvaderSpace.shieldCollected == 0)
        {
            spriteRenderer.sprite = normalEvader;
            Color yellowColor = HexToColor("#FFFF00");
            Vector3 newScale = new Vector3(3.0f, 3.0f, 3.0f);
            spriteRenderer.material.color = yellowColor;
            spriteRenderer.transform.localScale = newScale;
        }
        if (LevelSelector.chosenLevel != 1)
        {
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

        if (portalCount > 0 && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)))
        {
            shiftKeyNotPressed = false;
            shiftKey.SetActive(false);
            GameObject particleInstance = Instantiate(smoke, transform.position, Quaternion.identity);
            particleInstance.GetComponent<ParticleSystem>().Play();
            portalCount--;
            Text portalCountText = GameObject.Find("PortalCount").GetComponent<Text>();
            portalCountText.text = "x" + portalCount;
            Vector3 chaserPosition = this.transform.position; //69 24
            if (chaserPosition.x <= 72 && chaserPosition.y <= 20)
            {
                MoveToRandomPosition(new Vector2(115f, 45f));
            }
            if (chaserPosition.x <= 72 && chaserPosition.y >= 20)
            {
                MoveToRandomPosition(new Vector2(115f, 1f));
            }
            if (chaserPosition.x >= 72 && chaserPosition.y <= 20)
            {
                MoveToRandomPosition(new Vector2(30f, 45f));
            }
            if (chaserPosition.x >= 72 && chaserPosition.y >= 20)
            {
                MoveToRandomPosition(new Vector2(30f, 1f));
            }
        }

        if (!isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            if (normalLedgeSelected && platformCount > 0)
            {
                DroppedLedge = Instantiate(floorprefab, transform.position, Quaternion.identity);
                platformCount--;
                LedgeCount.text = "x" + platformCount;
                LedgeCount.gameObject.SetActive(true);
                isGrounded = true;
            }
            else if (iceLedgeSelected && icePlatformCount > 0)
            {
                DroppedLedge = Instantiate(iceFloorPrefab, transform.position, Quaternion.identity);
                icePlatformCount--;
                iceLedgeCount.text = "x" + icePlatformCount;
                iceLedgeCount.gameObject.SetActive(true);
                isGrounded = true;
            }
        }
        if (iceCollected && (Input.GetKeyDown(KeyCode.M)))
        {
            if (normalLedgeSelected)
            {
                iceHighlight.SetActive(true);
                normalLedgeSprite.SetActive(false);
                normalLedgeSelected = false;
                iceLedgeSelected = true;
            }
            else
            {
                iceHighlight.SetActive(false);
                normalLedgeSprite.SetActive(true);
                normalLedgeSelected = true;
                iceLedgeSelected = false;
            }
            hint.SetActive(false);
        }

        if(iceCollected && icePlatformCount==0){
            hint.SetActive(false);
        }

        float gameplayDuration = Time.time - survivalStartTime;
        if (gameplayDuration > 10f && shiftKeyNotPressed)
        {
            shiftKey.SetActive(true);
        }

        ShieldCount.text = "x" + EvaderSpace.shieldCollected;

        if (isCollidingWithLedge && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            Destroy(currentCollision.gameObject);
            platformCount++;
            totalPlatformCount++;
            LedgeCount.text = "x" + platformCount;
            isCollidingWithLedge = false;
            currentCollision = null;
        }
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

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        // rb.velocity = new Vector2(jumpVelocity, jumpForce);
        // System.out.println(rb.velocity.x);
        // Console. WriteLine(rb.velocity.x);
        // Scene currentScene = SceneManager.GetActiveScene();

        // if(currentScene.name == "Level1"){
        //     rb.AddForce(Vector2.up * jumpForce1, ForceMode2D.Impulse);
        // }else{
        //     rb.AddForce(Vector2.up * jumpForce2, ForceMode2D.Impulse);
        // }
        isGrounded = false;
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.CompareTag("icePoint"))
        {
            icePlatformCount = 5;
            totalIcePlatformCount = 5;
            Destroy(collision.gameObject);
            iceLedgeCount.text = "x" + icePlatformCount;
            iceLedgeCount.gameObject.SetActive(true);
            hint.SetActive(true);
            iceCollected = true;
            StartCoroutine(iceHighlightFlash());
        }
        else if (collision.gameObject.CompareTag("AddTeleport"))
        {
            collectTeleport.SetActive(false);
            portalCount += 5;
            totalPortalCount += 5;
            Text portalCountText = GameObject.Find("PortalCount").GetComponent<Text>();
            portalCountText.text = "x" + portalCount;
            StartCoroutine(PortalHighlightFlash());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Chaser") || collision.gameObject.CompareTag("Laser") || collision.gameObject.CompareTag("GhostChaser") || collision.gameObject.CompareTag("FrozenChaser"))
        {
            Evader.deathPosition = collision.transform.position;

            if (collision.gameObject.CompareTag("Laser"))
                lostReason = "laser";

            if(LevelSelector.chosenLevel != 1 && !hasCollidedWithChaser)
            {
                hasCollidedWithChaser = true;
                if (EvaderSpace.shieldCollected == 0)
                {
                    ShowGameOverHideTimer();
                    Debug.Log("set time called in collision");
                    TimerScript.setTime();
                }
                else
                {
                    EvaderSpace.shieldCollected -= 1;
                    
                    if (EvaderSpace.shieldCollected == 0)
                    {
                        spriteRenderer.sprite = normalEvader;
                        Color yellowColor = HexToColor("#FFFF00");
                        Vector3 newScale = new Vector3(3.0f, 3.0f, 3.0f);
                        spriteRenderer.material.color = yellowColor;
                        spriteRenderer.transform.localScale = newScale;
                    }

                    StartCoroutine(ShieldHighlightFlash());
                    StartCoroutine(RespawnPlayer());
                }
            }
        }
        else if (collision.gameObject.CompareTag("SafeLedge") && safeLedgeUsed == false && IsPlayerAboveLedge(collision.gameObject.transform.position.y))
        {
            onSafeLedge = true;
            safeLedgeUsed = true;
            chaserController.StopChasing();
            countdown.SetActive(true);
            countdownController.StartCountdown(5f);
            StartCoroutine(ResumeChasingAfterDelay(7f));
        }
        else
        {
            isGrounded = true;
        }
    }
    
    private IEnumerator PortalHighlightFlash()
    {
        portalHighlight.SetActive(true);
        SpriteRenderer portalHighlightRenderer = portalHighlight.GetComponent<SpriteRenderer>();
        // Flash the chaser red to remind the player
        Color originalColor = portalHighlightRenderer.color;
        for (int i = 0; i < 2; i++) // Flash 2 times
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
        for (int i = 0; i < 2; i++) // Flash 2 times
        {
            shieldHighlightRenderer.color = Color.red;
            yield return new WaitForSeconds(0.5f); // Flash duration
            shieldHighlightRenderer.color = originalColor;
            yield return new WaitForSeconds(0.5f); // Time between flashes
        }
        shieldHighlight.SetActive(false);
    }

    IEnumerator RespawnPlayer()
    {
        spriteRenderer.enabled = false;
        DestroyFireFloor();
        yield return new WaitForSeconds(1f);

        hasCollidedWithChaser = false;
        transform.position = new Vector2(89f, 45f);
        chaserSpriteRenderer.transform.position = new Vector2(35f, 25f);
        spriteRenderer.enabled = true;
    }

    void DestroyFireFloor(){
        GameObject[] fireFloorObjects = GameObject.FindGameObjectsWithTag("Fire");

        foreach (var obj in fireFloorObjects)
        {
            Destroy(obj);
        }
    }

    private IEnumerator iceHighlightFlash()
    {
        iceHighlight.SetActive(true);
        SpriteRenderer iceHighlightRenderer = iceHighlight.GetComponent<SpriteRenderer>();
        // Flash the chaser red to remind the player
        Color originalColor = Color.white;
        for (int i = 0; i < 2; i++) // Flash 2 times
        {
            iceHighlightRenderer.color = Color.red;
            yield return new WaitForSeconds(0.5f); // Flash duration
            iceHighlightRenderer.color = originalColor;
            yield return new WaitForSeconds(0.5f); // Time between flashes
        }
        iceHighlight.SetActive(false);
    }

    private bool IsPlayerAboveLedge(float ledgeY)
    {
        float playerY = transform.position.y;

        Debug.Log("Ledge position:" + ledgeY);
        Debug.Log("Player position:" + playerY);
        float verticalThreshold = 2.0f;

        return playerY > ledgeY + verticalThreshold;
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
        // Find all game objects with the tag "LedgePrefab"
        GameObject[] ledges = GameObject.FindGameObjectsWithTag("LedgePrefab");

        // Make each ledge invisible
        foreach (GameObject ledge in ledges)
        {
            // Assuming your LedgePrefab has a Renderer component
            Renderer renderer = ledge.GetComponent<Renderer>();

            // Check if the renderer is not null to avoid errors
            if (renderer != null)
            {
                // Set the object's visibility to false
                renderer.enabled = false;
            }
        }

        // Find all game objects with the tag "Fire"
        GameObject[] fireObjects = GameObject.FindGameObjectsWithTag("Fire");

        // Remove each fire object
        foreach (GameObject fireObject in fireObjects)
        {
            Renderer renderer = fireObject.GetComponent<Renderer>();
            // Check if the renderer is not null to avoid errors
            if (renderer != null)
            {
                // Set the object's visibility to false
                renderer.enabled = false;
            }
        }

        GameObject[] iceObjects = GameObject.FindGameObjectsWithTag("iceLedge");
        // Remove each fire object
        foreach (GameObject iceObject in iceObjects)
        {
            Renderer renderer = iceObject.GetComponent<Renderer>();
            // Check if the renderer is not null to avoid errors
            if (renderer != null)
            {
                // Set the object's visibility to false
                renderer.enabled = false;
            }
        }

        // Find the game object with the tag "Evader" and destroy it
        GameObject evaderTag = GameObject.FindGameObjectWithTag("Player");
        GameObject addTeleport = GameObject.FindGameObjectWithTag("AddTeleport");
        GameObject mapTag = GameObject.FindGameObjectWithTag("ledgeTileMap");
        GameObject chaserTag = GameObject.FindGameObjectWithTag("Chaser");
        GameObject icePoint = GameObject.FindGameObjectWithTag("icePoint");
        GameObject Portal1 = GameObject.FindGameObjectWithTag("Portal1");
        GameObject shift = GameObject.FindGameObjectWithTag("ShiftTag");
        GameObject frozenChaser = GameObject.FindGameObjectWithTag("FrozenChaser");
        GameObject GhostChaser = GameObject.FindGameObjectWithTag("GhostChaser");
        if (GhostChaser != null)
        {
            GhostChaser.SetActive(false);
        }

        if (frozenChaser != null)
        {
            frozenChaser.SetActive(false);
        }


        if (shift != null)
        {
            shift.SetActive(false);
        }


        if (icePoint != null)
        {
            icePoint.SetActive(false);
        }
        if (evaderTag != null)
        {
            evaderTag.SetActive(false);
        }
        if (addTeleport != null)
        {
            addTeleport.SetActive(false);
        }
        if (mapTag != null)
        {
            mapTag.SetActive(false);
        }
        if (chaserTag != null)
        {
            chaserTag.SetActive(false);
        }
        if (Portal1 != null)
        {
            Portal1.SetActive(false);
        }


        GameText.text = "GAME OVER";
        GameText.gameObject.SetActive(true);
        TimerTxt.gameObject.SetActive(false);
        chaserSpriteRenderer.sprite = smilingSprite;
        spriteRenderer.sprite = caughtSprite;
        RestartText.gameObject.SetActive(true);
        float survivalDuration = Time.time - survivalStartTime;
        string iceCount = "0";
        if (iceCollected)
            iceCount = (totalIcePlatformCount - icePlatformCount).ToString();
        StartCoroutine(sendDataScript.SendDataToGoogleSheets(survivalDuration.ToString(), Teleport.wormholeUsed, (totalPortalCount - portalCount).ToString(), lostReason,
        (totalPlatformCount - platformCount).ToString(), iceCount, EvaderSpace.totalShieldsCollected.ToString(), ChaserAI.timesStuck.ToString(), deathPosition.ToString()));
        Time.timeScale = 0f;
        portalCount = 0;
        platformCount = 0;
        icePlatformCount = 0;
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
        Invoke("activateGhost", 15.0f);
        Invoke("openWormhole", 4.0f);
    }

    void openWormhole()
    {
        if (LevelSelector.chosenLevel != 1 && !EvaderSpace.visited)
        {
            wormhole.gameObject.SetActive(true);
        }
    }

    void activateGhost()
    {
        if (LevelSelector.chosenLevel == 3 && ghostNotCalled)
        {
            ghostChaser.SetActive(true);
            AIDestinationSetter ghostTarget = ghostChaser.GetComponent<AIDestinationSetter>();
            ghostTarget.target = rb.transform;
            GameObject ogChaser = GameObject.Find("Chaser");
            AIPath ogChaserSpeed = ogChaser.GetComponent<AIPath>();
            ogChaserSpeed.maxSpeed = 20;
            ghostNotCalled = false;
            Invoke("removeGhost", 10.0f);
        }
    }

    void removeGhost()
    {
        GameObject ogChaser = GameObject.Find("Chaser");
        AIPath ogChaserSpeed = ogChaser.GetComponent<AIPath>();
        ogChaserSpeed.maxSpeed = 80;
        ghostChaser.SetActive(false);
        // Destroy(ghostChaser);
    }

    public void RestartButtonClicked()
    {
        Time.timeScale = 1f;
        timerController.RestartTime();
        hasCollidedWithChaser = false;
        EvaderSpace.shield = false;
        EvaderSpace.shieldCollected = 0;
        EvaderSpace.visited = false;
        TimerScript.AreWeReturningToTheScene = false;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void NextButtonClicked()
    {
        Time.timeScale = 1f;
        hasCollidedWithChaser = false;
        EvaderSpace.shield = false;
        EvaderSpace.shieldCollected = 0;
        EvaderSpace.visited = false;
        TimerScript.AreWeReturningToTheScene = false;
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Level1")
        {
            LevelSelector.chosenLevel = 2;
            SceneManager.LoadScene("Level2");
        }
        else
        {
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
        hex = hex.TrimStart('#');
        Color color = new Color();
        ColorUtility.TryParseHtmlString("#" + hex, out color);
        return color;
    }
}
