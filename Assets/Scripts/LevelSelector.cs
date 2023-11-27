using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class LevelSelector : MonoBehaviour
{
    public int level;
    // public TextMeshPro levelText;
    public static int chosenLevel = 1;

    void Start()
    {
        
    }

    // Update is called once per frame
    public void OpenScene()
    {
        if (level.ToString() == "1")
        {
            SceneManager.LoadScene("Level1");
        }
        else if(level.ToString() == "2") {
            chosenLevel = 2;
            SceneManager.LoadScene("Level2");
        }
        else if(level.ToString() == "3") {
            chosenLevel = 3;
            SceneManager.LoadScene("Level3");
        }
        else if(level.ToString() == "4") {
            chosenLevel = 4;
            SceneManager.LoadScene("Tutorial1");
        }
        else if(level.ToString() == "5") {
            chosenLevel = 5;
            SceneManager.LoadScene("Tutorial2");
        }
        else if(level.ToString() == "6") {
            chosenLevel = 6;
            SceneManager.LoadScene("Tutorial3");
        }
    }
}
