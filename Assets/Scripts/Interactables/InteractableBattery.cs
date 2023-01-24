using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBattery : Interactable2
{
    public override void Interact()
    {
        base.Interact();

        Destroy(this.gameObject);
    }
}
