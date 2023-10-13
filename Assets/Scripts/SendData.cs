using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SendData : MonoBehaviour
{
    public IEnumerator SendDataToGoogleSheets(string survivalDuration, string evaderTeleportUsage) 
    {
        string webAppUrl = "https://script.google.com/macros/s/AKfycby4A5LTheYt-hUK78bz5A3nHHOI6uo_lFOCCNmg5jIrJytszskrBf89zpC02RuKKz-L/exec";
        WWWForm form = new WWWForm();
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