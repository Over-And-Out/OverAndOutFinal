using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTelefono : Interactable2
{
    [SerializeField] private AudioSource playerAudioSource = default;
    public AudioSource PlayerAudioSource { get => playerAudioSource; }
     [SerializeField] private AudioClip sonido1 = default;
    public AudioClip sonidoIni { get => sonido1; }

    public continuaraCanvas continuara;
    public override void Interact()
    {
        base.Interact();
        continuara.gameObject.SetActive(true);
        PlayerAudioSource.PlayOneShot(sonido1);
    }
}
