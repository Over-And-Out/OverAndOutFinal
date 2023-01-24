using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText = default;

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
        healthText.text = currentHealth.ToString("00");
    }
}
