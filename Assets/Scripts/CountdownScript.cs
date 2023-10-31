using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CountDownScript : MonoBehaviour
{
    private float initialTime = 5f;
    private float timeLeft;
    public Text Countdown;
    private bool countingDown = false;


    void Start()
    {
        timeLeft = initialTime;
        UpdateTimerText();
    }

   void Update()
    {
        if (countingDown && timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            countingDown = false;
            timeLeft = 0;
            Countdown.text = "Chasing resumes!";
        }
    }

    void UpdateTimerText()
    {
        int roundedTime = Mathf.CeilToInt(timeLeft); // Round up to the nearest integer.
        Countdown.text = roundedTime.ToString();
    }

    public void StartCountdown(float duration)
    {
        initialTime = duration; // Set the countdown duration
        timeLeft = initialTime;
        countingDown = true;
    }
}
