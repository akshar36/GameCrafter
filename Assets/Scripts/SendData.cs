using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SendData : MonoBehaviour
{
    public IEnumerator SendDataToGoogleSheets(string survivalDuration, string evaderTeleportUsage, string gameOverReason) 
    {
        string webAppUrl = "https://script.google.com/macros/s/AKfycbxMdeZH1GQHtQtdHdG28iV2KNGWI-KVbMNTnfNBVWey8eU_IOXkhwk9RKLFw4Grbg3B/exec";
        WWWForm form = new WWWForm();
        form.AddField("map", LevelSelector.chosenLevel);
        form.AddField("survivalDuration", survivalDuration);
        form.AddField("evaderTeleportUsage", evaderTeleportUsage);
        form.AddField("gameOverReason", gameOverReason);

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