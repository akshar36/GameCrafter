using System.Collections;
using UnityEngine;

public class Chaser : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public float minIdleTime = 1.0f;
    public float maxIdleTime = 3.0f;

    private bool isMoving = true;
    private float idleTime;
    private float elapsedTime;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetRandomIdleTime();
    }

    void Update()
    {
        if (isMoving)
        {
            Debug.Log("Chaser is Moving");
            isMoving = false;
        }
        else
        {
            Debug.Log("Chaser Stopped Moving");
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= idleTime)
            {
                int randomDirection = Random.Range(0, 2);
                float horizontalInput = (randomDirection == 0) ? -1.0f : 1.0f;

                Move(horizontalInput);

                elapsedTime = 0f;
                SetRandomIdleTime();
            }
        }
    }

    void Move(float horizontalInput)
    {
        Vector2 movement = new Vector2(horizontalInput * moveSpeed * Time.deltaTime, rb.velocity.y);
        rb.velocity = movement;
    }

    void SetRandomIdleTime()
    {
        idleTime = Random.Range(minIdleTime, maxIdleTime);
    }
}
