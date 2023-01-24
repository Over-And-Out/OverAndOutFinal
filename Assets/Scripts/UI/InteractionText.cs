using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InteractionText : MonoBehaviour
{
    public static InteractionText instance;
    public TextMeshProUGUI interText;

    private void Awake() 
    {
        instance = this;
    }
}
