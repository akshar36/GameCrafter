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
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    private GameObject bullet;
    private float angle = 90f;
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
        if(LevelSelector.chosenLevel == 3)
            StartCoroutine(ShootBulletsRandomly());
    }

    public void StopChasing()
    {
        isChasing = false;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f; // Stop any angular velocity.
        movespeed = 0f;
        rb.Sleep(); // Put the rigidbody to sleep to clear any forces.
    }

    IEnumerator ShootBulletsRandomly()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            ShootBullets();
        }
    }

    void ShootBullets()
    {
        for (int i = 0; i <8; i++)
        {
            angle = angle + 45*i;  
            Debug.Log("angle " + angle);
            Vector2 bulletDirection = Quaternion.Euler(0, 0, angle) * Vector2.up;
            Vector2 spawnPosition = (Vector2)transform.position + bulletDirection * 6f;

            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
            
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

            rb.velocity = bulletDirection * bulletSpeed;

            // var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            // bullet.GetComponent<Rigidbody2D>().velocity = bulletSpawnPoint.up * bulletSpeed;

        }
        angle = -180;
    }
}
