using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableKey : Interactable
{
    [Header("Custom Sounds")]
    [SerializeField] private AudioClip pickUpKey = default;

    public override void OnFocus()
    {
        print("Focus on " + gameObject.name);
    }

    public override void OnInteract(GameObject itemsContainer, ref List<HandItem> allPlayerObjects)
    {
        FirtsPersonController controller = FirtsPersonController.Instance;

        controller.HasKey = true;
        controller.PlayerAudioSource.PlayOneShot(pickUpKey);

        Destroy(this.gameObject);
    }

    public override void OnLoseFocus()
    {
        print("Lost focus on " + gameObject.name);
    }
}
