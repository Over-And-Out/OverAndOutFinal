using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePanel : Interactable
{
    public objetivoCanvas objetivo;
    public activarTelefono telefono;

    [SerializeField]private encenderLuces luces;
    [SerializeField] private AudioSource playerAudioSource = default;
    public AudioSource PlayerAudioSource { get => playerAudioSource; }
     [SerializeField] private AudioClip sonido1 = default;
    public AudioClip sonidoIni { get => sonido1; }
    public override void OnFocus()
    {
        print("Focus on " + gameObject.name);
    }

    public override void OnInteract(GameObject itemsContainer, ref List<HandItem> allPlayerObjects)
    {
        PlayerAudioSource.PlayOneShot(sonido1);
        luces.gameObject.SetActive(true);
        objetivo.gameObject.SetActive(true);
        telefono.gameObject.SetActive(true);

    }

    public override void OnLoseFocus()
    {
        print("Lost focus on " + gameObject.name);
    }
}
