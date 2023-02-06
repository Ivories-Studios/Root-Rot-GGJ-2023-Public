using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerObject : UnitObject
{
    [SerializeField] private PlayerUIEvents _playerUIEvents;
    [SerializeField] private RootMovementController _playerMovementController;

    protected override void Awake()
    {
        base.Awake();
        _playerMovementController = GetComponent<RootMovementController>();
    }

    private void Start()
    {
        transform.position = GameState.Checkpoint;
    }

    protected override void Update()
    {
        if (GameState.IsPaused)
        {
            return;
        }
        base.Update();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        _playerUIEvents.OnHealthChange(UnitStats.Health, UnitStats.MaxHealth);
        ImpulseSource.GenerateImpulse();
    }

    protected override void Die()
    {
        _playerUIEvents.OnDie();
        _playerMovementController.ClearAllRoots();
        StartCoroutine(RestartScene());
    }

    IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(2f);
        transform.position = GameState.Checkpoint;
        UnitStats.Health = UnitStats.MaxHealth;
        UnitStats.IsDead = false;
        _playerUIEvents.OnHealthChange(UnitStats.Health, UnitStats.MaxHealth);
    }
}
