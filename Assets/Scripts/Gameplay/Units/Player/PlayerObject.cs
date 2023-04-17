using System.Collections;
using UnityEngine;

public sealed class PlayerObject : UnitObject
{
    [SerializeField] private PlayerUIEvents _playerUIEvents;
    [SerializeField] private RootMovementController _playerMovementController;
    [SerializeField] private PlayerOutlineScript _playerOutlineScript;

    [SerializeField] private float _damageForce;

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
        ImpulseSource.GenerateImpulse(_damageForce);
        _playerOutlineScript.TakeDamage();
    }

    public void Heal(int health)
    {
        UnitStats.Health += health;

        if (UnitStats.Health > UnitStats.MaxHealth)
            UnitStats.Health = UnitStats.MaxHealth;

        _playerUIEvents.OnHealthChange(UnitStats.Health, UnitStats.MaxHealth);
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
