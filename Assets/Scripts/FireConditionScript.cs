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
        if (collision.tag == "Player"){
            contactDuration += Time.deltaTime;
            Debug.Log("contact "+ contactDuration);
            if (contactDuration < 2f && !hasTouchedLava)
                if(EvaderSpace.shieldCollected == 0){
                    Evader.lostReason = "lava";
                    Evader.deathPosition = collision.transform.position;
                    // Call the setTime method from TimerScript.
                    TimerScript.setTime();

                    //This one will not automatively call the time reset function.
                    evaderController.ShowGameOverHideTimer();
                }
                else if(EvaderSpace.shieldCollected > 0)
                {
                    DeductShield();
                }
            }
    }

    void DeductShield()
    {
        hasTouchedLava = true;
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

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // Reset contactDuration and hasTouchedLava when player exits the trigger
            contactDuration = 0;
            hasTouchedLava = false;
        }
    }


    IEnumerator RespawnPlayer(){
        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(1f);
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
