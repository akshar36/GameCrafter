using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SendData : MonoBehaviour
{
    public IEnumerator SendDataToGoogleSheets(string survivalDuration, string evaderTeleportUsage) 
    {
        string webAppUrl = "https://script.google.com/macros/s/AKfycby09Hzor8b7QT_EDbRWtJi3LsQNrbKZxRJeatlEfP_SKvD8YhPBFJNOi5YCi0yOwwvU/exec";
        WWWForm form = new WWWForm();
        form.AddField("map", LevelSelector.chosenLevel);
        form.AddField("survivalDuration", survivalDuration);
        form.AddField("evaderTeleportUsage", evaderTeleportUsage);

        UnityWebRequest www = UnityWebRequest.Post(webAppUrl, form);
        yield return www.SendWebRequest();

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