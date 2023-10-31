using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerScript : MonoBehaviour
{
    private static float TimeLeft;
    public static bool AreWeReturningToTheScene = false;
    public Text GameText;
    public Text TimerTxt;
    public GameObject RestartTxt;
    private bool startTime = false;
    public SendData sendDataScript;

    void Start()
    {
        RestartTxt.gameObject.SetActive(false);
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
        StartCoroutine(sendDataScript.SendDataToGoogleSheets(survivalDuration.ToString(), Teleport.teleportUsageCount.ToString(), "won"));
        Color pinkColor = HexToColor("#FFC0CB");
        GameText.color = pinkColor;
        GameText.gameObject.SetActive(true);
        TimerTxt.gameObject.SetActive(false);
        RestartTxt.gameObject.SetActive(true);
        Time.timeScale = 0f;
        SceneManager.LoadScene("LevelSelection");
        setTime();
        
    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;
        Debug.Log("currentTime "+ currentTime);
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        Debug.Log("seconds "+ seconds);

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