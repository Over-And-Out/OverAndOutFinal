using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePanel : Interactable
{
    public objetivoCanvas objetivo;

    [SerializeField]private encenderLuces luces;
    [SerializeField] private AudioSource playerAudioSource = default;
    public AudioSource PlayerAudioSource { get => playerAudioSource; }
     [SerializeField] private AudioClip sonido = default;
    public AudioClip sonidoIni { get => sonido; }
    public override void OnFocus()
    {
        print("Focus on " + gameObject.name);
    }

    public override void OnInteract(GameObject itemsContainer, ref List<HandItem> allPlayerObjects)
    {
        PlayerAudioSource.PlayOneShot(sonido);
        luces.gameObject.SetActive(true);
        objetivo.gameObject.SetActive(true);
    }

    public override void OnLoseFocus()
    {
        print("Lost focus on " + gameObject.name);
    }
}
