using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections; 

public class TimerScript : MonoBehaviour
{
    private static float TimeLeft;
    public static bool AreWeReturningToTheScene = false;
    public Text GameText;
    public Text TimerTxt;
    public GameObject RestartTxt;
    public GameObject PlayAgainTxt;
    public GameObject NextLevelTxt;
    private bool startTime = false;
    public SendData sendDataScript;

    void Start()
    {
        RestartTxt.gameObject.SetActive(false);
        PlayAgainTxt.gameObject.SetActive(false);
        if(NextLevelTxt){
            NextLevelTxt.gameObject.SetActive(false);
        }
        Debug.Log("set time called on start");
        if (!AreWeReturningToTheScene){
            setTime();
        }
        updateTimer(TimeLeft);
    }

    public static void setTime(){
        switch(LevelSelector.chosenLevel){
            case 1:
                TimeLeft = 39f;
                break;
            case 2:
                TimeLeft = 49f;
                break;
            case 3:
                TimeLeft = 59f;
                break;
        }
    }

    void Update()
    {
        if(startTime)
        {
            if(TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
                updateTimer(TimeLeft);
            }
            else {
                showGameWin();
                TimeLeft = 0;
            }
        }
    }

    void showGameWin(){
        Time.timeScale = 0f;

        // Find all game objects with the tag "LedgePrefab"
        GameObject[] ledges = GameObject.FindGameObjectsWithTag("LedgePrefab");

        // Make each ledge invisible
        foreach (GameObject ledge in ledges)
        {
            // Assuming your LedgePrefab has a Renderer component
            Renderer renderer = ledge.GetComponent<Renderer>();

            // Check if the renderer is not null to avoid errors
            if (renderer != null)
            {
                // Set the object's visibility to false
                renderer.enabled = false;
            }
        }

        // Find all game objects with the tag "Fire"
        GameObject[] fireObjects = GameObject.FindGameObjectsWithTag("Fire");

        // Remove each fire object
        foreach (GameObject fireObject in fireObjects)
        {
            Renderer renderer = fireObject.GetComponent<Renderer>();
            // Check if the renderer is not null to avoid errors
            if (renderer != null)
            {
                // Set the object's visibility to false
                renderer.enabled = false;
            }
        }
        GameObject[] iceObjects = GameObject.FindGameObjectsWithTag("iceLedge");
        // Remove each fire object
        foreach (GameObject iceObject in iceObjects)
        {
            Renderer renderer = iceObject.GetComponent<Renderer>();
            // Check if the renderer is not null to avoid errors
            if (renderer != null)
            {
                // Set the object's visibility to false
                renderer.enabled = false;
            }
        }

        // Find the game object with the tag "Evader" and destroy it
        GameObject evaderTag = GameObject.FindGameObjectWithTag("Player");
        GameObject addTeleport = GameObject.FindGameObjectWithTag("AddTeleport");
        GameObject mapTag = GameObject.FindGameObjectWithTag("ledgeTileMap");
        GameObject chaserTag = GameObject.FindGameObjectWithTag("Chaser");
        GameObject icePoint = GameObject.FindGameObjectWithTag("icePoint");
        GameObject Portal1 = GameObject.FindGameObjectWithTag("Portal1");
        GameObject shift = GameObject.FindGameObjectWithTag("ShiftTag");
        GameObject frozenChaser = GameObject.FindGameObjectWithTag("FrozenChaser");
        GameObject GhostChaser = GameObject.FindGameObjectWithTag("GhostChaser");
        if (GhostChaser != null)
        {
            GhostChaser.SetActive(false);
        }


        if (frozenChaser != null)
        {
            frozenChaser.SetActive(false);
        }

        if (shift != null)
        {
            shift.SetActive(false);
        }

        if (icePoint != null)
        {
            icePoint.SetActive(false);
        }
        if (addTeleport != null)
        {
            addTeleport.SetActive(false);
        }
        if (mapTag != null)
        {
            mapTag.SetActive(false);
        }
        if (Portal1 != null)
        {
            Portal1.SetActive(false);
        }

        GameText.text = "YOU WIN";
        float survivalDuration = Time.time - Evader.survivalStartTime;

        string platformCount = "0";
        string portalCount = "0";
        string iceCount = "0";

        if(LevelSelector.chosenLevel == 1){
            platformCount = (EvaderLevel1.totalPlatformCount - EvaderLevel1.platformCount).ToString();
            portalCount = (EvaderLevel1.totalPortalCount-EvaderLevel1.portalCount).ToString();
        }
        else{
            platformCount = (Evader.totalPlatformCount - Evader.platformCount).ToString();
            portalCount = (Evader.totalPortalCount-Evader.portalCount).ToString();
            if(Evader.iceCollected)
                iceCount = (Evader.totalIcePlatformCount - Evader.icePlatformCount).ToString();
        }

        if(LevelSelector.chosenLevel <= 3){
            StartCoroutine(sendDataScript.SendDataToGoogleSheets(survivalDuration.ToString(), Teleport.wormholeUsed, portalCount, "won", 
                platformCount, iceCount, EvaderSpace.totalShieldsCollected.ToString(), ChaserAI.timesStuck.ToString(), ""));

        }

        GameText.gameObject.SetActive(true);
        PlayAgainTxt.gameObject.SetActive(true);
        if(NextLevelTxt){
             NextLevelTxt.gameObject.SetActive(true);
        }
        TimerTxt.gameObject.SetActive(false);

        Evader.portalCount = 0;
        Evader.platformCount = 0;
        Evader.icePlatformCount = 0;
    }

   

    void updateTimer(float currentTime)
    {
        currentTime += 1;
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        TimerTxt.text = string.Format("{0:00}:{1:00} seconds left to win", minutes, seconds);
    }

    Color HexToColor(string hex)
    {
        // Remove the '#' character if it's included
        hex = hex.TrimStart('#');

        // Parse the hex string to a Color object
        Color color = new Color();
        ColorUtility.TryParseHtmlString("#" + hex, out color);

        return color;
    }
    public void StartTime()
    {
        startTime = true;
    }

    public void RestartTime()
    {
        setTime();
        startTime = true;
    }

}

    // void OnCollisionStay2D(Collision2D collision)
    // {

    //     if (collision.gameObject.CompareTag("LedgePrefab")) // Detect when collision ends
    //     {
    //         if(Input.GetKeyDown(KeyCode.N)){
    //             Destroy(collision.gameObject);
    //             platformCount++;
    //             LedgeCount.text = "x " + platformCount;
    //             Debug.Log("Player collided with the portal");
    //         }
    //     }
    // }