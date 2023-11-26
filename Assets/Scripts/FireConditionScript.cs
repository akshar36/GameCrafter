using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FireConditionScript : MonoBehaviour
{
    private Evader evaderController;
    public Sprite hitOne;
    public Sprite hitTwo;
    public Sprite hitThree;
    private SpriteRenderer spriteRenderer;
    private float contactStartTime;
    private bool touchingFire = false;
    private float contactDuration;
    private float currentContactStartTime;
    private float x;
    public Sprite normalEvader;
    private bool hasTouchedLava = false;
    // Start is called before the first frame update
    void Start()
    {
        GameObject evader = GameObject.Find("Evader");
        evaderController = evader.GetComponent<Evader>();
        spriteRenderer = evader.GetComponent<SpriteRenderer>();
        hasTouchedLava = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !EvaderSpace.shield){
            currentContactStartTime = Time.time;
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (collision.tag == "Player" && !hasTouchedLava)
        {
            contactDuration += Time.deltaTime;
            if (contactDuration < 2f){
                if(EvaderSpace.shieldCollected == 0){
                    Evader.lostReason = "lava";
                    Evader.deathPosition = collision.transform.position;
                    TimerScript.setTime();
                    evaderController.ShowGameOverHideTimer();
                }
                else if(EvaderSpace.shieldCollected > 0)
                {
                    hasTouchedLava = true;
                    StartCoroutine(ShieldHighlightFlash());
                    DeductShield();
                }
            }
        }
    }

    void DeductShield()
    {
        Debug.Log("before" + EvaderSpace.shieldCollected.ToString());
        EvaderSpace.shieldCollected -= 1;
        Debug.Log("after" + EvaderSpace.shieldCollected.ToString());

        StartCoroutine(RespawnPlayer());
        spriteRenderer.transform.position = new Vector2(89f, 45f);

        if (EvaderSpace.shieldCollected == 0)
        {
            spriteRenderer.sprite = normalEvader;
            Color yellowColor = HexToColor("#FFFF00");
            Vector3 newScale = new Vector3(3.0f, 3.0f, 3.0f);
            spriteRenderer.material.color = yellowColor;
            spriteRenderer.transform.localScale = newScale;
        }

        spriteRenderer.enabled = true;

        // Delay the reset of hasTouchedLava
        StartCoroutine(ResetHasTouchedLava());
    }


    IEnumerator ResetHasTouchedLava()
    {
        // Wait for a certain period before allowing another shield deduction
        yield return new WaitForSeconds(1f);
        hasTouchedLava = false;
    }
    
     private IEnumerator ShieldHighlightFlash()
    {
        evaderController.shieldHighlight.SetActive(true);
        SpriteRenderer shieldHighlightRenderer = evaderController.shieldHighlight.GetComponent<SpriteRenderer>();
        // Flash the chaser red to remind the player
        Color originalColor = shieldHighlightRenderer.color;
        for (int i = 0; i < 2; i++) // Flash 2 times
        {
            shieldHighlightRenderer.color = Color.red;
            yield return new WaitForSeconds(0.5f); // Flash duration
            shieldHighlightRenderer.color = originalColor;
            yield return new WaitForSeconds(0.5f); // Time between flashes
        }
        evaderController.shieldHighlight.SetActive(false);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // Reset contactDuration and hasTouchedLava when player exits the trigger
            contactDuration = 0;
        }
    }


    IEnumerator RespawnPlayer(){
        spriteRenderer.enabled = false;
        DestroyFireFloor();

        yield return new WaitForSeconds(1f);
    }

    void DestroyFireFloor(){
        GameObject[] fireFloorObjects = GameObject.FindGameObjectsWithTag("FireFloor");

        foreach (var obj in fireFloorObjects)
        {
            Destroy(obj);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        // Check if the trigger is with the tile (you might use a tag or layer to identify it)
        if(collision.tag == "Player" && !EvaderSpace.shield)
        {
                contactDuration += Time.time - contactStartTime;
        }
    }


    Color HexToColor(string hex)
    {
        hex = hex.TrimStart('#');
        Color color = new Color();
        ColorUtility.TryParseHtmlString("#" + hex, out color);
        return color;
    }
    
}
