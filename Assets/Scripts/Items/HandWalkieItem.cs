using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandWalkieItem : HandItem
{
    public GameObject WalkieCanvas;
    public GameObject InGameMenu;

    public override void UseItem(FirtsPersonController player)
    {
        if(!WalkieCanvas.activeSelf && !InGameMenu.activeSelf)
        {
            WalkieCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            WalkieCanvas.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
