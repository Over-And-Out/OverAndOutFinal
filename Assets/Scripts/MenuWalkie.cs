using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuWalkie : MonoBehaviour
{
    public GameObject WalkieCanvas;
    public bool actionRunning = false;
    public GameObject monster;
    public Transform[] waypoints;
    public GameObject Subtitulos;
    public TMP_Text TextoSubtitulos;

    [SerializeField] private AudioSource playerAudioSource = default;
    public AudioSource PlayerAudioSource { get => playerAudioSource; }
    [SerializeField] private AudioClip sonidoInicio = default;
    [SerializeField] private AudioClip sonidoFin = default;
    public AudioClip sonidoIni { get => sonidoInicio; }
    public AudioClip sonidoF { get => sonidoFin; }

    public void UbicacionMonstruo()
    {
        
        if (!actionRunning)
        {

            PlayerAudioSource.PlayOneShot(sonidoIni);
            ActivarSubtitulos();
            CambiarTextoSubtitulos("Timmy: " + DialogosTimmyUbicacionMonstruo());
            actionRunning = true;

            Invoke("RespuestaUbicacion", 2);
        }
        WalkieCanvas.SetActive(false);
        PlayerAudioSource.PlayOneShot(sonidoF);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Tareas()
    {
        if (!actionRunning)
        {
            ActivarSubtitulos();
            CambiarTextoSubtitulos("Timmy: Que hago ahora?");
            actionRunning = true;

            Invoke("RespuestaTarea", 2);
        }
        WalkieCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    void RespuestaUbicacion()
    {
        int masCercana = 0;
        float distanciaMasCercana = 100000;
        for (int i = 0; i < waypoints.Length; i++)
        {
            float distancia;
            if (!(Mathf.Abs(waypoints[i].position.y - monster.transform.position.y) < 3)) 
            {
                distancia = 100000;
            }
            else
            {
                distancia = Vector3.Distance(waypoints[i].position, monster.transform.position);
                if (distancia < distanciaMasCercana)
                {
                    masCercana = i;
                    distanciaMasCercana = distancia;
                }
            }
            
        }
        CambiarTextoSubtitulos("Joel: " + DialogosJoelUbicacionMonstruo(waypoints[masCercana].name));
        Invoke("DesactivarSubtitulos",2);
    }

    void RespuestaTarea()
    {
        CambiarTextoSubtitulos("Joel: Intenta encender las luces");
        Invoke("DesactivarSubtitulos", 2);
    }

    void ActivarSubtitulos()
    {
        Subtitulos.SetActive(true);
    }

    void CambiarTextoSubtitulos(string Texto)
    {
        TextoSubtitulos.text = Texto;
    }

    void DesactivarSubtitulos()
    {
        Subtitulos.SetActive(false);
        actionRunning = false;
    }

    //Selecciona aleatoriamente una frase para solicitar la ubicacion del monstruo a Joel
    string DialogosTimmyUbicacionMonstruo()
    {
        int nAleatorio = UnityEngine.Random.Range(1, 5);
        switch (nAleatorio)
        {
            case 1: return "Sabes donde esta el monstruo?";
            case 2: return "Ves al monstruo?"; 
            case 3: return "Joel lo he perdido! Dime donde esta!"; 
            case 4: return "Joel dime la posicion del monstruo!";
            default: return "";
        }
    }

    string DialogosJoelUbicacionMonstruo(string ubicacion)
    {
        int nAleatorio = UnityEngine.Random.Range(1, 4);
        switch (nAleatorio)
        {
            case 1: return "El monstruo esta en " + ubicacion;
            case 2: return "Esta en " + ubicacion + "corre!";
            case 3: return "No lo veo!";
            default: return "";
        }
    }
}
