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
        if (level.ToString() == "0")
        {
            SceneManager.LoadScene("Tutorial1");
        }
        else if (level.ToString() == "1")
        {
            Debug.Log("xxx");
            SceneManager.LoadScene("SampleScene");
        }
        else if(level.ToString() == "2") {
            chosenLevel++;
            SceneManager.LoadScene("SampleScene2");
        }
    }
}
