using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SendData : MonoBehaviour
{
    public IEnumerator SendDataToGoogleSheets(string survivalDuration, string teleportUsed, 
    string gameOverReason, string logsPlaced, string shieldsCollected, string timesChaserStuck, string deathPosition) 
    {
        string webAppUrl = "https://script.google.com/macros/s/AKfycbyQjgYY9FBJrkx7ozjrvZyuW2Vp30rmSzafnhuAeccDiCqk-oYqWWGFZfPlftTegVgH/exec";
        WWWForm form = new WWWForm();
        form.AddField("map", LevelSelector.chosenLevel);
        form.AddField("survivalDuration", survivalDuration);
        form.AddField("teleportUsed", teleportUsed);
        form.AddField("gameOverReason", gameOverReason);
        form.AddField("logsPlaced", logsPlaced);
        form.AddField("shieldsCollected", shieldsCollected);
        form.AddField("timesChaserStuck", timesChaserStuck);
        form.AddField("deathPosition", deathPosition);

        using UnityWebRequest www = UnityWebRequest.Post(webAppUrl, form);
        {
        yield return www.SendWebRequest();
        }

        if(www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError) 
        {
            Debug.LogError(www.error);
        } 
        else 
        {
            Debug.Log("Data Sent Successfully");
        }

    }
}