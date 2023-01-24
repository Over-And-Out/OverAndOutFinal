using System.Net.Http;
using System.Net.WebSockets;
using System.Net;
using System.Diagnostics;
using System;
using System.Runtime.CompilerServices;
using System.Net.NetworkInformation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Interactables
{
    public class camaraInteraccion : MonoBehaviour
    {
        private int rango = 3;

        public GameObject linterna;
        private float bateria = 100;

        // Update is called once per frame
        void Update()
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rango, LayerMask.GetMask("Interact")))
            {
                InteractionText.instance.interText.text = "Presiona E para recoger";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //UnityEngine.Debug.Log("Hola");
                    linterna.GetComponent<InteractableLantern>().cantBateria += bateria;
                    hit.transform.GetComponent<Interactable2>().Interact();

                    //BarraBateria.instance.batterySlider.value = bateria;
                    //BarraBateria.instance.batteryText.text = "Bateria: " + bateria + "%";
                }
            }
            else if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rango, LayerMask.GetMask("PanelLuz")))
            {
                InteractionText.instance.interText.text = "Presiona Click Izq para interactuar";
                if (Input.GetMouseButtonDown(0))
                {
                    hit.transform.GetComponent<Interactable2>().Interact();
                }
            }
            else
            {
                InteractionText.instance.interText.text = "";
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * rango);
        }
    }
}