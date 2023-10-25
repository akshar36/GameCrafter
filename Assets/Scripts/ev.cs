// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System;
// using UnityEngine.UI;
// using UnityEngine.Tilemaps;
// using UnityEngine.SceneManagement;
// using TMPro;
// public class Evader : MonoBehaviour
// {
//     public float moveSpeed = 5.0f;
//     public float jumpForce = 8.0f;
//     private Rigidbody2D rb;
//     private bool isGrounded;
//     private ChaserAI chaserController;
//     private TimerScript timerController;
//     private int platformCount = 3;
//     public GameObject floorprefab;
//     private GameObject chaser;
//     public TextMeshPro GameText;
//     public GameObject RestartText;
//     public TextMeshPro LedgeCount;
//     public TextMeshPro TimerTxt;
//     private SpriteRenderer spriteRenderer;
//     private SpriteRenderer chaserSpriteRenderer;
//     public Sprite caughtSprite;
//     public Sprite smilingSprite;
//     public Sprite shieldSprite;
//     private bool evaderMoved = false;
//     private GameObject DroppedLedge;
//     private bool isColliding = false;
//     private GameObject LedgePrefab;
//     void Start()
//     {
//         HideGameOverShowTimer();
//         rb = GetComponent<Rigidbody2D>();
//         spriteRenderer = GetComponent<SpriteRenderer>();
//         GameObject chaser = GameObject.Find("Chaser");
//         GameObject timer = GameObject.Find("TimerTxt");
//         chaserSpriteRenderer = chaser.GetComponent<SpriteRenderer>();
//         chaserController = chaser.GetComponent<ChaserAI>();
//         timerController = timer.GetComponent<TimerScript>();
//         Debug.Log("EvaderSpace.val " + EvaderSpace.shield);
//         if(EvaderSpace.shield) {
//             Debug.Log("shield activated");
//             spriteRenderer.sprite = shieldSprite;
//         }
//     }

//     void Update()
//     {
//         float moveInput = Input.GetAxis("Horizontal");
//         Vector2 moveDirection = new Vector2(moveInput, 0);
//         if(rb.velocity.magnitude > 5f && !evaderMoved){
//             Debug.Log("rb.velocity.magnitude "+ rb.velocity.magnitude);
//             evaderMoved = true;
//             StartRunning();
//         }

//         rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);

//         if (isGrounded && Input.GetKeyDown(KeyCode.W))
//         // if (Input.GetKey(KeyCode.W))
//         {
//             Jump();
//         }
//         if (Input.GetKeyDown(KeyCode.M)) {
//             if(DroppedLedge != null) {
//                 DroppedLedge.transform.Rotate(Vector3.forward * 90.0f);
//             }

//         }

//         if(!isGrounded && Input.GetKeyDown(KeyCode.Space) && platformCount>0)
//         {
//             DroppedLedge = Instantiate(floorprefab, transform.position, Quaternion.identity);
//             platformCount--;
//             LedgeCount.text = "x " + platformCount;
//             LedgeCount.gameObject.SetActive(true);
//             isGrounded = true;
//         }
//     }

//     void Jump()
//     {
//         rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
//         isGrounded = false;
//     }

//     void OnCollisionEnter2D(Collision2D collision)
//     {
//         if(collision.gameObject.CompareTag("Chaser"))
//         {
//             ShowGameOverHideTimer();
//         } else{
//             isGrounded = true;
//         }
//     }

//     void OnCollisionStay2D(Collision2D collision)
//     {

//         if (collision.gameObject.CompareTag("LedgePrefab")) // Detect when collision ends
//         {
//             if(Input.GetKeyDown(KeyCode.N)){
//                 Destroy(collision.gameObject);
//                 platformCount++;
//                 LedgeCount.text = "x " + platformCount;
//                 Debug.Log("Player collided with the portal");
//             }
//         }
//     }

//     void ShowGameOverHideTimer()
//     {
//         GameText.text = "Game Over";
//         GameText.gameObject.SetActive(true);
//         TimerTxt.gameObject.SetActive(false);
//         chaserSpriteRenderer.sprite = smilingSprite;
//         spriteRenderer.sprite = caughtSprite;
//         RestartText.gameObject.SetActive(true);
//         Time.timeScale = 0f;
//     }

//     void HideGameOverShowTimer()
//     {
//         GameText.gameObject.SetActive(false);
//         TimerTxt.gameObject.SetActive(true);
//         RestartText.gameObject.SetActive(false);
//     }

//     public void StartRunning()
//     {
//         chaserController.StartChasing();
//         timerController.StartTime();
//     }

//     public void RestartButtonClicked()
//     {
//         Time.timeScale = 1f;
//         Debug.Log("REstart");
//         Scene currentScene = SceneManager.GetActiveScene();
//         SceneManager.LoadScene(currentScene.name);
//     }
// }