using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Linterna : MonoBehaviour
{

    public Light luzLinterna;
    public bool activeLight;    

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            activeLight = !activeLight;
            
            if(activeLight == true)
            {
                luzLinterna.enabled = true;
            }

            if(activeLight == false)
            {
                luzLinterna.enabled = false;
            }
        }
    }
}
