using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(collision.tag == "Player"){
            contactDuration += Time.deltaTime;
            Debug.Log("contact "+ contactDuration);
            if(contactDuration < 2f){
                if(EvaderSpace.shieldCollected == 0){
                    Evader.lostReason = "lava";
                    Evader.deathPosition = collision.transform.position;
                    evaderController.ShowGameOverHideTimer();
                }
                else if(EvaderSpace.shieldCollected > 0){
                    spriteRenderer.sprite = hitTwo;
                    EvaderSpace.shieldCollected -=1;
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
