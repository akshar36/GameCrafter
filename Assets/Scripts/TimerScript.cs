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
        GameText.text = "YOU WIN";
        float survivalDuration = Time.time - Evader.survivalStartTime;
        Evader.portalCount = 0;
        Evader.platformCount = 0;
        Evader.icePlatformCount= 0;
        string platformCount = "0";
        string portalCount = "0";
        string iceCount = "0";

        if(LevelSelector.chosenLevel == 1){
            platformCount = (10 - EvaderLevel1.platformCount).ToString();
            portalCount = (5-EvaderLevel1.portalCount).ToString();
        }
        else{
            platformCount = (5 - Evader.platformCount).ToString();
            portalCount = (5-Evader.portalCount).ToString();
            if(Evader.iceCollected)
                iceCount = (5 - Evader.icePlatformCount).ToString();
        }

        StartCoroutine(sendDataScript.SendDataToGoogleSheets(survivalDuration.ToString(), Teleport.wormholeUsed, portalCount, "won", 
            platformCount, iceCount, EvaderSpace.totalShieldsCollected.ToString(), ChaserAI.timesStuck.ToString(), ""));
        GameText.gameObject.SetActive(true);
        PlayAgainTxt.gameObject.SetActive(true);
        if(NextLevelTxt){
             NextLevelTxt.gameObject.SetActive(true);
        }
        TimerTxt.gameObject.SetActive(false);
        Time.timeScale = 0f;
        setTime();
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