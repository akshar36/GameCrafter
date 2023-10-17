using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EvaderPractice : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpForce = 8.0f;
    private Rigidbody2D rb;
    private bool isGrounded;
    private TimerScriptPractice timerController;
    // private int platformCount = 3;
    public GameObject floorprefab;
    public TextMeshPro GameText;
    public GameObject RestartText;
    public TextMeshPro LedgeCount;
    public TextMeshPro TimerTxt;
    private bool evaderMoved = false;
    private float i = 5.0f;
    public Canvas PopUp1;
    private GameObject DroppedLedge;

    void Start()
    {
        if (i > 0)
        {
            i -= Time.deltaTime;
        }
        else
        {
            PopUp1.gameObject.SetActive(false);
        }
        LedgeCount.text = "x " + "\u221E";
        HideGameOverShowTimer();
        rb = GetComponent<Rigidbody2D>();
        GameObject timer = GameObject.Find("TimerTxt");
        timerController = timer.GetComponent<TimerScriptPractice>();
    }

    void Update()
    {
        if (!PopUp1.gameObject.activeSelf)
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

            if (isGrounded && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
            {
                Jump();
            }

            if(Input.GetKeyDown(KeyCode.M)) {
                    if(DroppedLedge != null) {
                        DroppedLedge.transform.Rotate(Vector3.forward * 90.0f);
                    }
            }

            if (!isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                DroppedLedge = Instantiate(floorprefab, transform.position, Quaternion.identity);
                // platformCount--;
                // LedgeCount.text = "x " + platformCount;
                // LedgeCount.gameObject.SetActive(true);
                isGrounded = true;
            }
        }
        else
        {
            if (Input.anyKeyDown)
            {
                PopUp1.gameObject.SetActive(false);
            }
        }

    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Chaser"))
        {
            ShowGameOverHideTimer();
        }else if (collision.gameObject.CompareTag("checkpoint")) {
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            Debug.Log("hit the ground");
            isGrounded = true;
        }

    }

    void OnCollisionStay2D(Collision2D collision)
    {
         if (collision.gameObject.CompareTag("LedgePrefab"))
         {
             if(Input.GetKeyDown(KeyCode.N)){
                 Destroy(collision.gameObject);
             }
         }
     }

    void ShowGameOverHideTimer()
    {
        GameText.text = "Game Over";
        GameText.gameObject.SetActive(true);
        TimerTxt.gameObject.SetActive(false);
        RestartText.gameObject.SetActive(true);
        Time.timeScale = 0f;
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
        Debug.Log("Restart");
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

     public void BackButtonClicked()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("LevelSelection");
        Debug.Log("Back clicked");
    }
}
