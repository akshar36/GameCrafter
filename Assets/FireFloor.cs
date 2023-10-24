using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FireFloor : MonoBehaviour
{
    public GameObject objectToSpawn; // The GameObject to spawn
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
            Vector2 collisionPoint = collision.GetContact(0).point;
            Debug.Log(collisionPoint);
            GameObject spawnedObject = Instantiate(objectToSpawn, collisionPoint, Quaternion.identity);
            StartCoroutine(RemoveTileAfterDelay(spawnedObject, 5f));
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
