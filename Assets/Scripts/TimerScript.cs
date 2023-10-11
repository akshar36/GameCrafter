using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerScript : MonoBehaviour
{
    public float TimeLeft;
    public TextMeshPro GameText;
    public TextMeshPro TimerTxt;
    private bool startTime = false;
    void Start()
    {
        updateTimer(99);
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
        GameText.text = "You Win";
        Color pinkColor = HexToColor("#FFC0CB");
        GameText.color = pinkColor;
        GameText.gameObject.SetActive(true);
        TimerTxt.gameObject.SetActive(false);
        Time.timeScale = 0f;
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