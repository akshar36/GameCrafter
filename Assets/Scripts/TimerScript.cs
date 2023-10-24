using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerScript : MonoBehaviour
{
    public static float TimeLeft = 49f;
    public Text GameText;
    public Text TimerTxt;
    public GameObject RestartTxt;
    private bool startTime = false;
    void Start()
    {
        RestartTxt.gameObject.SetActive(false);
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
                showGameWin();
                TimeLeft = 0;
            }
        }
    }

    void showGameWin(){
        GameText.text = "YOU WIN";
        Color pinkColor = HexToColor("#FFC0CB");
        GameText.color = pinkColor;
        GameText.gameObject.SetActive(true);
        TimerTxt.gameObject.SetActive(false);
        RestartTxt.gameObject.SetActive(true);
        Time.timeScale = 0f;
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