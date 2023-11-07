using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerScriptForFlashGame : MonoBehaviour
{
    public float TimeLeft;
    public Text GameText;
    public Text TimerTxt;
    private bool startTime = false;
    public static string totalShieldsCollected = "0";
    private string sceneToReturnTo = "Level1";
    private string sceneToReturnTo2 = "Level2";
    private string sceneToReturnTo3 = "Level3";

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
                totalShieldsCollected = EvaderSpace.shieldCollected.ToString();
                TimerScript.AreWeReturningToTheScene = true;
                if(LevelSelector.chosenLevel == 1){
                    SceneManager.LoadScene(sceneToReturnTo);
                    } 
                else if(LevelSelector.chosenLevel == 2){
                    SceneManager.LoadScene(sceneToReturnTo2);
                }
                else if(LevelSelector.chosenLevel == 3){
                    SceneManager.LoadScene(sceneToReturnTo3);
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
        hex = hex.TrimStart('#');
        Color color = new Color();
        ColorUtility.TryParseHtmlString("#" + hex, out color);
        return color;
    }
    public void StartTime()
    {
        startTime = true;
    }
}
