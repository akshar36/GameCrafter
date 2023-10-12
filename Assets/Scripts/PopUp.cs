using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public Canvas PopUp1;
    private float i = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (i > 0)
        {
            i -= Time.deltaTime;
        }
        else
        {
            PopUp1.gameObject.SetActive(false);
        }
       
    }
}
