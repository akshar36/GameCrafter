using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using TMPro;
using Random = UnityEngine.Random;

public class EvaderTutorial : MonoBehaviour
{
    public Text ShieldCount;
    private int shields = 2;
    public float moveSpeed = 5.0f;
    public float jumpForce = 11.0f;
    public float jumpForce1 = 8.0f;
    public float jumpForce2 = 11.0f;
    private Rigidbody2D rb;
    private bool isGrounded;
    private TimerScript timerController;
    public static int platformCount = 10;
    public static int totalPlatformCount = 10;
    public GameObject floorprefab;
    public Text GameText;
    public GameObject RestartText;
    public GameObject BackButton;
    public Text LedgeCount;
    public Text TimerTxt;
    private SpriteRenderer spriteRenderer;
    public Sprite caughtSprite;
    public Sprite smilingSprite;
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
    public static int totalPortalCount = 5;
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
    bool isCollidingWithLedge = false;
    Collision2D currentCollision;
    private bool isSkeyShown = false;
    Vector2? deathPosition = null;
    private GameObject collectTeleport;
    private Vector2 respawnPosition = new Vector2(31f, 45f);

    public GameObject portalHighlight;
    private bool recollectUsed;
    private bool jumpUsed;

    void Start()
    {
        jumpUsed = false;
        recollectUsed = false;
        portalHighlight.SetActive(false);
        shields = 2;
        portalCount = 5;
        JumpSpace.SetActive(false);
        Recollect.SetActive(false);
        isSkeyShown = false;
        HideGameOverShowTimer();
        platformCount = 10;
        totalPlatformCount = 10;
        rb = GetComponent<Rigidbody2D>();
        shiftKey.SetActive(false);
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameObject timer = GameObject.Find("TimerTxt");
        collectTeleport = GameObject.Find("add_teleport");
        collectTeleport.SetActive(true);
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

            if (LevelSelector.chosenLevel != 4 && portalCount >0 && ((Input.GetKeyDown(KeyCode.LeftShift)) || (Input.GetKeyDown(KeyCode.RightShift)))){
                shiftKey.SetActive(false);
                Debug.Log("playing");
                GameObject particleInstance = Instantiate(smoke, transform.position, Quaternion.identity);
                particleInstance.GetComponent<ParticleSystem>().Play();
                portalCount --;
                Text portalCountText = GameObject.Find("PortalCount").GetComponent<Text>();
                portalCountText.text = "x"+portalCount;
                Vector3 chaserPosition = this.transform.position; //69 24
                if (chaserPosition.x <= 72 && chaserPosition.y <= 20)
                {
                 MoveToRandomPosition(new Vector2(110f, 45f));
                }
                if (chaserPosition.x <= 72 && chaserPosition.y >= 20)
                {
                 MoveToRandomPosition(new Vector2(110f, 1f));
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

            if (!isGrounded && Input.GetKeyDown(KeyCode.Space) && platformCount > 0)
            {
                JumpSpace.SetActive(false);
                jumpUsed = true;
                DroppedLedge = Instantiate(floorprefab, transform.position, Quaternion.identity);
                platformCount--;
                LedgeCount.text = "x " + platformCount;
                LedgeCount.gameObject.SetActive(true);
                isGrounded = true;
                if (LevelSelector.chosenLevel != 4 && shiftNotclicked)
                    StartCoroutine(removeShift(7.0f));
            }
        

        // if(LevelSelector.chosenLevel == 2 && Time.time > 10 && !EvaderSpace.visited){
        //     wormhole.gameObject.SetActive(true);
        // }

        if(jumpUsed && !recollectUsed){
            if(isCollidingWithLedge && !isSkeyShown){
                Recollect.SetActive(true);
                isSkeyShown = true;
            }
        }else{
                Recollect.SetActive(false);
                isSkeyShown = false;
            }

        if (isCollidingWithLedge && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            Recollect.SetActive(false);
            recollectUsed = true;
            Destroy(currentCollision.gameObject);
            platformCount++;
            totalPlatformCount++;
            LedgeCount.text = "x " + platformCount;
            isCollidingWithLedge = false;
            currentCollision = null;  
        }
    }

    private IEnumerator RemoveRawImagesAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        yield return new WaitForSeconds(2.0f);
        if(!jumpUsed)
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
        if(collision.gameObject.CompareTag("AddTeleport"))
        {
            collectTeleport.SetActive(false);
            portalCount += 5;
            totalPortalCount += 5;
            Text portalCountText = GameObject.Find("PortalCount").GetComponent<Text>();
            portalCountText.text = "x" + portalCount;
            StartCoroutine(PortalHighlightFlash());
        }
        else{
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
        TimerScript.AreWeReturningToTheScene = false;
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
        if(currentScene.name == "Tutorial1"){
            //LevelSelector.chosenLevel = 2;
            SceneManager.LoadScene("Tutorial2");
        }else if(currentScene.name == "Tutorial2"){
            //LevelSelector.chosenLevel = 3;
            SceneManager.LoadScene("Tutorial3");
        }else if(currentScene.name == "Tutorial3"){
            SceneManager.LoadScene("Level1");
        }
        TimerScript.AreWeReturningToTheScene = false;
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
