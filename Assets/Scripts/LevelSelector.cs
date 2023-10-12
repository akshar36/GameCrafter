using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelector : MonoBehaviour
{
    public int level;
    public TextMeshProUGUI levelText;
    void Start()
    {
        levelText.text = level.ToString();   
    }

    // Update is called once per frame
    public void OpenScene()
    {
        if (level.ToString() == "1")
        {
            SceneManager.LoadScene("Tutorial1");
        }
        //if(level.ToString() == "2") {
        //    SceneManager.LoadScene("SampleScene");
        //}
        else if(level.ToString() == "2") {
            SceneManager.LoadScene("SampleScene2");
        } 
        else if (level.ToString() == "3"){
            SceneManager.LoadScene("SampleScene3");
        }
    }
}
