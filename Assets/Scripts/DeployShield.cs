using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployShield : MonoBehaviour {
    public GameObject shieldPrefabs;
    public float respawnTime = 1.0f;
    private Vector2 screenBounds;

    // Use this for initialization
    void Start () {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        Debug.Log(screenBounds.x);
        Debug.Log(screenBounds.y);
        StartCoroutine(asteroidWave());
    }
    private void spawnObjects(){
            GameObject a = Instantiate(shieldPrefabs) as GameObject;
            a.transform.position = new Vector2(screenBounds.x * 1.3f , Random.Range(-screenBounds.y, screenBounds.y));
    }
    IEnumerator asteroidWave(){
        while(true){
            yield return new WaitForSeconds(respawnTime);
            spawnObjects();
        }
    }
}