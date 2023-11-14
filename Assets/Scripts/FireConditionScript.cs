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
    // Start is called before the first frame update
    void Start()
    {
        GameObject evader = GameObject.Find("Evader");
        evaderController = evader.GetComponent<Evader>();
        spriteRenderer = evader.GetComponent<SpriteRenderer>();
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
            if(contactDuration < 2f){
                if(EvaderSpace.shieldCollected == 0){
                    Evader.lostReason = "lava";
                    Evader.deathPosition = collision.transform.position;
                    // Call the setTime method from TimerScript.
                    TimerScript.setTime();

                    //This one will not automatively call the time reset function.
                    evaderController.ShowGameOverHideTimer();
                }
                else if(EvaderSpace.shieldCollected > 0 && currentScene.name == "Level2")
                {
                    spriteRenderer.sprite = hitTwo;
                    spriteRenderer.transform.position = new Vector3(75f, 27f, 0);
                    EvaderSpace.shieldCollected -=1;
                }
                else if (EvaderSpace.shieldCollected > 0 && currentScene.name == "Level3")
                {
                    spriteRenderer.sprite = hitTwo;
                    spriteRenderer.transform.position = new Vector3(75f, 25f, 0);
                    EvaderSpace.shieldCollected -= 1;
                }
            }
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
    
}
