using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerScriptPractice : MonoBehaviour
{
    public static float TimeLeft = 9f;
    public Text GameText;
    public Text TimerTxt;
    private bool startTime = false;
    private string sceneToReturnTo = "Level1"; // The name of the scene you want to return to
    private string sceneToReturnTo2 = "Level2";
    private string sceneToReturnTo3 = "Level3";

    void Start()
    {
        TimeLeft = 9f;
        updateTimer(TimeLeft);
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
            else
            {
                TimeLeft = 0;
                TimerScript.AreWeReturningToTheScene = true;
                GameObject evader = GameObject.Find("Evader");
                EvaderSpace evaderSpaceController = evader.GetComponent<EvaderSpace>();
                evaderSpaceController.SetSceneBack();
            }
        }
    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
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
