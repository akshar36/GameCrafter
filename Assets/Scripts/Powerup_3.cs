using UnityEngine;
using System.Collections;

public class ShootBullet : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 20f;
    private GameObject bullet;
    private float angle = 90f;
    void Start()
    {
    }


    void ShootBullets()
    {
        for (int i = 0; i <8; i++)
        {
            angle = angle + 45*i;  // 45 degrees between each bullet
            Debug.Log("angle " + angle);
            Vector2 bulletDirection = Quaternion.Euler(0, 0, angle) * Vector2.up;
            Vector2 spawnPosition = (Vector2)bulletSpawnPoint.position + bulletDirection * 6f;

            bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
            
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

            rb.velocity = bulletDirection * bulletSpeed;

            // var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            // bullet.GetComponent<Rigidbody2D>().velocity = bulletSpawnPoint.up * bulletSpeed;

        }
        angle = -180;
    }
}
