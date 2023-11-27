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
        if (collision.tag == "Player")
        {
            hasTouchedLava = true;
            contactDuration += Time.deltaTime;

            if(EvaderSpace.shieldCollected == 0){
                Evader.lostReason = "lava";
                Evader.deathPosition = collision.transform.position;
                TimerScript.setTime();
                evaderController.ShowGameOverHideTimer();
            }
            RespawnPlayer();
        }
    }
    private void OnTriggerExit2D(Collider2D collision){
        if (collision.tag == "Player"){
            DeductShield();
            hasTouchedLava = false;
        } 
   }


    void DeductShield()
    {
        Debug.Log("before" + EvaderSpace.shieldCollected.ToString());
        EvaderSpace.shieldCollected -= 1;
        Debug.Log("after" + EvaderSpace.shieldCollected.ToString());
    }

    void RespawnPlayer(){
        DestroyFireFloor();
        spriteRenderer.transform.position = new Vector2(89f, 45f);

        if (EvaderSpace.shieldCollected == 0)
        {
            spriteRenderer.sprite = normalEvader;
            Color yellowColor = HexToColor("#FFFF00");
            Vector3 newScale = new Vector3(3.0f, 3.0f, 3.0f);
            spriteRenderer.material.color = yellowColor;
            spriteRenderer.transform.localScale = newScale;
        }
    }

    void DestroyFireFloor(){
        GameObject[] fireFloorObjects = GameObject.FindGameObjectsWithTag("Fire");

        foreach (var obj in fireFloorObjects)
        {
            Destroy(obj);
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
