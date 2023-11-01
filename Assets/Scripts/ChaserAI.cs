using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;

public class ChaserAI : MonoBehaviour
{
    public Transform target;
    public float movespeed = 900f;
    public float nextWayPointDistance = 3.0f;
    private Vector2 previousPosition;
    Path path;
    int currentWayPoint;
    bool reachedEndofPath = false;
    private bool isChasing = false;
    Seeker seeker;
    Rigidbody2D rb;

    private float timeNotMoving = 0f;
    private float destroyThreshold = 5.0f;
     private float destroyThresholdInitial = 4.0f;
    private float ledgeDestroyRadius = 3.0f;
    public Text LedgeCount;
    public Sprite angryChaser;
    public Sprite normalChaser;
    private SpriteRenderer chaserSpriteRenderer;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        previousPosition = transform.position;
        InvokeRepeating("UpdatePath", 0f, 1f);

        GameObject chaser = GameObject.Find("Chaser");
        chaserSpriteRenderer = chaser.GetComponent<SpriteRenderer>();
    }

    void UpdatePath()
    {
        if (isChasing)
        {
            if(seeker.IsDone()){
            seeker.StartPath(rb.position, target.position, OnPathComplete);
            }
        }

    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWayPoint=0;
        }
    }

    void Update()
    {
        if (isChasing)
        {
            if(path == null)
            {
                return;
            }
            if(currentWayPoint >= path.vectorPath.Count)
            {
                reachedEndofPath = true;
                return;
            }
            else {
                reachedEndofPath = false;
            }

            Vector2 direction = ((Vector2) path.vectorPath[currentWayPoint] - rb.position).normalized;
            Vector2 force = direction*movespeed*Time.deltaTime;

            rb.AddForce(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

            if(distance< nextWayPointDistance)
            {
                currentWayPoint++;
            }

            CheckForMovement();
        }
    }

    public void StartChasing()
    {
        isChasing = true;
        movespeed = 900f;
    }

    public void StopChasing()
    {
        isChasing = false;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f; // Stop any angular velocity.
        movespeed = 0f;
        rb.Sleep(); // Put the rigidbody to sleep to clear any forces.
    }

    void CheckForMovement()
    {
        // Check if the position has changed
        if (Vector2.Distance(transform.position, previousPosition) < 0.01f) // Threshold can be adjusted
        {
            // Position hasn't changed significantly, increase timer
            timeNotMoving += Time.deltaTime;
        }
        else
        {
            // Position changed, reset timer
            timeNotMoving = 0f;
        }

        // Update previous position
        previousPosition = transform.position;

        // Check if timeNotMoving has reached the threshold
        if (timeNotMoving >= destroyThresholdInitial)
        {
            MakeChaserBigger();
        }
        if (timeNotMoving >= destroyThreshold)
        {
            // Destroy nearby LedgePrefabs
            DestroyNearbyLedges();
            // Reset timer
            timeNotMoving = 0f;
            chaserSpriteRenderer.sprite = normalChaser;
            Vector3 newScale = new Vector3(3.0f, 3.0f, 3.0f);
            chaserSpriteRenderer.transform.localScale = newScale;
        }
    }

    void DestroyNearbyLedges()
    {
        // Find all LedgePrefabs within the radius
        Collider2D[] ledges = Physics2D.OverlapCircleAll(transform.position, ledgeDestroyRadius);
        foreach (Collider2D collider in ledges)
        {
            if (collider.tag == "LedgePrefab")
            {
                Destroy(collider.gameObject);
                if(LevelSelector.chosenLevel == 1)
                    EvaderLevel1.platformCount++;
                else
                    Evader.platformCount++;
                    LedgeCount.text = "x " + Evader.platformCount;
            }
        }
    }

    void MakeChaserBigger(){
        chaserSpriteRenderer.sprite = angryChaser;
        Vector3 newScale = new Vector3(5.0f, 5.0f, 5.0f);
        chaserSpriteRenderer.transform.localScale = newScale;
    }

}
