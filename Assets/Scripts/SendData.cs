using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SendData : MonoBehaviour
{
    public IEnumerator SendDataToGoogleSheets(string survivalDuration, string wormholeUsed, string teleportCount,
    string gameOverReason, string logsPlaced, string iceLogsPlaced, string shieldsCollected, string timesChaserStuck, string deathPosition) 
    {
        string webAppUrl = "https://script.google.com/macros/s/AKfycbw9KYLqsMnwRC112qYkIq8EijRm1M1ppzPxmcCzk3eFxPSO_W_FX7dvmFOJEz33K3s/exec";
        WWWForm form = new WWWForm();
        form.AddField("level", LevelSelector.chosenLevel);
        form.AddField("wormholeUsed", wormholeUsed);
        form.AddField("teleportUsedCount", teleportCount);
        form.AddField("survivalDuration", survivalDuration);
        form.AddField("gameOverReason", gameOverReason);
        form.AddField("logsPlacedCount", logsPlaced);
        form.AddField("iceLogsPlacedCount", iceLogsPlaced);
        form.AddField("shieldsCollectedCount", shieldsCollected);
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