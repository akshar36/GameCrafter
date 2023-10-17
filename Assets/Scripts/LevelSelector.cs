using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelector : MonoBehaviour
{
    public int level;
    public TextMeshProUGUI levelText;
    public static int chosenLevel = 1;

    void Start()
    {
        levelText.text = level.ToString();   
    }

    // Update is called once per frame
    public void OpenScene()
    {

        if (level.ToString() == "0")
        {
            SceneManager.LoadScene("Tutorial1");
        }
        else if (level.ToString() == "1")
        {
            SceneManager.LoadScene("SampleScene");
        }
        else if(level.ToString() == "2") {
            chosenLevel++;
            SceneManager.LoadScene("SampleScene2");
        }
    }
}
