using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalShow : MonoBehaviour
{
    public GameObject portal = null;
    void Start()
    {
        StartCoroutine(HideAndShowAfterDelay(10f));
    }


    private IEnumerator HideAndShowAfterDelay(float delay)
    {
        // portal.SetActive(false);

        // Wait for the specified delay in seconds
        yield return new WaitForSeconds(delay);
        Debug.Log("Please Show");
        portal.SetActive(true);
    }
}
