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

    void Start()
    {
        HideGameOverShowTimer();
        rb = GetComponent<Rigidbody2D>();
        GameObject timer = GameObject.Find("TimerTxt");
        timerController = timer.GetComponent<TimerScriptPractice>();
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
}
