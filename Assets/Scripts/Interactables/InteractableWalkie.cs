using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableWalkie : Interactable
{
    public activarMonstruo monstruo;
    public encenderLuces luces;
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
        if (!CheckWalkieInPlayer(allPlayerObjects))
        {
            GameObject walkie = Instantiate(prefab) as GameObject;
            HandWalkieItem itemToAdd = walkie.GetComponent<HandWalkieItem>();

            walkie.transform.parent = itemsContainer.transform;
            walkie.gameObject.transform.localPosition = itemPosition;
            walkie.transform.localRotation = itemRotation;
            walkie.SetActive(false);

            Destroy(this.gameObject);
            PlayerAudioSource.PlayOneShot(sonido);
            monstruo.gameObject.SetActive(true);
            luces.gameObject.SetActive(false);
            allPlayerObjects.Add(itemToAdd);
        }
    }

    private bool CheckWalkieInPlayer(List<HandItem> allPlayerObjects)
    {
        foreach (var item in allPlayerObjects)
        {
            if (item is HandWalkieItem)
                return true;
        }

        return false;
    }

    public override void OnLoseFocus()
    {
        print("Lose focus on " + gameObject.name);
    }
}
