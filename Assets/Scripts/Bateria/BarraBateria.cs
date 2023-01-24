using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarraBateria : MonoBehaviour
{
    public static BarraBateria instance;

    public Slider batterySlider;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
}
