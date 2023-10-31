using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

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

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        previousPosition = transform.position;
        InvokeRepeating("UpdatePath", 0f, 1f);
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
            // hits = Physics2D.OverlapBoxAll(transform.position, new Vector2(tilemap.cellBounds.size.x, tilemap.cellBounds.size.y), 0f, enemyLayer);
            // foreach (Collider2D hit in hits)
            // {
            //     // Get the position of the tile in world coordinates
            //     Vector3Int cellPosition = tilemap.WorldToCell(hit.transform.position);
            //     Debug.Log("workin");
            //     // Change the tile to the orange tile
            //     tilemap.SetTile(cellPosition, orangeTile);
            // }
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
}
