using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FireFloor : MonoBehaviour
{
    public GameObject objectToSpawn; // The GameObject to spawn
    private Vector2 temp = new Vector2(0f, 0f);
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
   
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Chaser"){
            GameObject spawnedObject = null;
            Vector2 collisionPoint = collision.GetContact(0).point;
            Debug.Log(collisionPoint);
            Vector2 roundedVector = new Vector2(Mathf.Round(collisionPoint.x), Mathf.Round(collisionPoint.y));
            if(temp != roundedVector){ 
                spawnedObject = Instantiate(objectToSpawn, collisionPoint, Quaternion.identity);
            temp = roundedVector;
            }
            StartCoroutine(RemoveTileAfterDelay(spawnedObject, 3f));
        }
    }
    private IEnumerator RemoveTileAfterDelay(GameObject tile, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (tile != null)
        {
            Destroy(tile);
        }
    }
}
