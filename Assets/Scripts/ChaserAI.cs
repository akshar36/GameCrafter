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
    private float destroyThreshold = 3.0f;
    private float ledgeDestroyRadius = 3.0f;
    public Text LedgeCount;
    public Sprite angryChaser;
    public Sprite normalChaser;
    private SpriteRenderer chaserSpriteRenderer;

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

    void CheckForMovement()
    {
        // Check if the position has changed
        if (Vector2.Distance(transform.position, previousPosition) < 0.02f)
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

        if (timeNotMoving >= destroyThreshold)
        {
            MakeChaserBigger();
            // Destroy nearby LedgePrefabs
            StartCoroutine(DestroyNearbyLedges());
        }
    }

    IEnumerator DestroyNearbyLedges()
    {
        yield return new WaitForSeconds(1.0f);
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
        // Reset timer
        timeNotMoving = 0f;
        chaserSpriteRenderer.sprite = normalChaser;
        Vector3 newScale = new Vector3(3.0f, 3.0f, 3.0f);
        chaserSpriteRenderer.transform.localScale = newScale;
    }

    private void MakeChaserBigger(){
        chaserSpriteRenderer.sprite = angryChaser;
        Vector3 newScale = new Vector3(5.0f, 5.0f, 5.0f);
        chaserSpriteRenderer.transform.localScale = newScale;
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
