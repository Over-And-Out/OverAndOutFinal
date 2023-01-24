using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour
{
    private int rango = 2;

    private void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rango)){
            
            if(hit.collider.GetComponent<Escondido>() == true)
            {
                if(Input.GetKeyDown(KeyCode.E))
                {
                     hit.collider.GetComponent<Escondido>().entra = true;
                }
            }

        }
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.green;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * rango);
    }
}
