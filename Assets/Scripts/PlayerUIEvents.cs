using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerUIEvents", menuName = "ScriptableObjects/PlayerUIEvents", order = 1)]
public class PlayerUIEvents : ScriptableObject
{
    public UnityEvent<float, float> OnHealthChanged;
    public UnityEvent OnMenuPress;
    public UnityEvent OnDead;

    public void OnHealthChange(float currentHealth, float maxHealth)
    {
        OnHealthChanged.Invoke(currentHealth, maxHealth);
    }

    public void OnMenu()
    {
        OnMenuPress.Invoke();
    }

    public void OnDie()
    {
        OnDead.Invoke();
    }
}
