using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private PlayerUIEvents _playerUIEvents;
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        _playerUIEvents.OnHealthChanged.AddListener(OnHealthChange);
    }

    private void OnDisable()
    {
        _playerUIEvents.OnHealthChanged.RemoveListener(OnHealthChange);
    }

    private void OnHealthChange(float health, float maxHealth)
    {
        _slider.maxValue = maxHealth;
        _slider.value = maxHealth - health;
    }
}
