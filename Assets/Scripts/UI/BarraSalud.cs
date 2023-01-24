using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarraSalud : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText = default;
    public static  BarraSalud instance;
    public Slider HealthSlider;
    private float Salud = 100;

    private void OnEnable()
    {
        FirtsPersonController.OnDamage += UpdateHealth;
        FirtsPersonController.OnHeal += UpdateHealth;

    }

    private void OnDisable()
    {
        FirtsPersonController.OnDamage -= UpdateHealth;
        FirtsPersonController.OnHeal -= UpdateHealth;
    }

    private void Start()
    {
        UpdateHealth(100);
    }

    private void UpdateHealth(float currentHealth)
    {
        healthText.text = "Salud: " + currentHealth.ToString("00") + "%";

        BarraSalud.instance.HealthSlider.maxValue = 100;
        Salud = Mathf.Clamp(Salud, 0, 100);
        BarraSalud.instance.HealthSlider.value = (float)currentHealth;
    }

    private void Awake()
    {
        instance = this;
    }
}
