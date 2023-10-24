using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerScriptPractice : MonoBehaviour
{
    public float TimeLeft;
    public Text GameText;
    public Text TimerTxt;
    private bool startTime = false;
    private string sceneToReturnTo = "SampleScene"; // The name of the scene you want to return to
    private string sceneToReturnTo2 = "SampleScene2";
    public static float mainSceneTimeLeft;

    void Start()
    {
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
                if(LevelSelector.chosenLevel == 1){
                    // Evader.AreWeReturningToTheScene = true;
                    Evader.AreWeReturningToTheScene = true;
                    TimerScript.TimeLeft = TimerScriptPractice.mainSceneTimeLeft;
                    SceneManager.LoadScene(sceneToReturnTo);
                    } else{
                    TimerScript.TimeLeft = TimerScriptPractice.mainSceneTimeLeft;
                    SceneManager.LoadScene(sceneToReturnTo2);
                }
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
