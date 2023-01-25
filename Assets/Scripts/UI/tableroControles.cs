using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tableroControles : Interactable2
{
    public controlesCanvas controles;
    public override void Interact()
    {
        base.Interact();

        controles.gameObject.SetActive(true);
    }
}

