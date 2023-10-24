using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EvaderPractice : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpForce = 8.0f;
    private Rigidbody2D rb;
    private TimerScriptPractice timerController;
    public GameObject floorprefab;
    public Text TimerTxt;
    private bool evaderMoved = false;
    private float i = 5.0f;
    private GameObject DroppedLedge;
    public GameObject highJump; 
    public GameObject fire; 
    public GameObject highJumpPowerPopUp;  
    public GameObject firePowerPopUp;  

    void Start()
    {
        HideGameOverShowTimer();
        rb = GetComponent<Rigidbody2D>();
        GameObject timer = GameObject.Find("TimerTxt");
        timerController = timer.GetComponent<TimerScriptPractice>();
        firePowerPopUp.SetActive(false);
        highJumpPowerPopUp.SetActive(false);
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Vector2 moveDirection = new Vector2(moveInput, 0);
        if (rb.velocity.magnitude > 5f && !evaderMoved)
        {
            Debug.Log("rb.velocity.magnitude " + rb.velocity.magnitude);
            evaderMoved = true;
            StartRunning();
        }

        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void HideGameOverShowTimer()
    {
        TimerTxt.gameObject.SetActive(true);
    }

    public void StartRunning()
    {
        timerController.StartTime();
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Entered Collission");

        if(collision.gameObject.CompareTag("HighJump"))
        {
            Debug.Log("Entered high");

            highJump.SetActive(false);
            
            // Display the popup message
            highJumpPowerPopUp.SetActive(true);
            StartCoroutine(HidePopup(2.0f, highJumpPowerPopUp));
        } else if(collision.gameObject.CompareTag("Fire")){
            fire.SetActive(false);
            Debug.Log("fire");

            
            // Display the popup message
            firePowerPopUp.SetActive(true);
            StartCoroutine(HidePopup(2.0f, firePowerPopUp));
        }

    }

    // Coroutine to hide the popup after a delay
    IEnumerator HidePopup(float delay, GameObject popUp)
    {
        yield return new WaitForSeconds(delay);
        popUp.SetActive(false);
    }
}
