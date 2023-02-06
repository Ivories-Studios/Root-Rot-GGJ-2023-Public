using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Teams
{
    Player,
    Enemy
}

public abstract class UnitObject : MonoBehaviour
{
    public Teams Team;
    public UnitStats UnitStats;
    [SerializeField] private List<Ability> _abilities = new List<Ability>();
    private Ability[] _activeAbilities;
    [HideInInspector] public CinemachineImpulseSource ImpulseSource;
    public Animator Animator;
    private Rigidbody2D _rigidbody2D;

    protected virtual void Awake()
    {
        UnitStats.Health = UnitStats.MaxHealth;
        ImpulseSource = GetComponent<CinemachineImpulseSource>();
        _activeAbilities = new Ability[_abilities.Count];
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        if (GameState.IsPaused)
        {
            return;
        }
        UnitStats.ReduceTimers();
    }

    public void BeginCastAbility(int index, Vector2 target)
    {
        _activeAbilities[index] = Instantiate(_abilities[index], transform.position, Quaternion.identity);
        _activeAbilities[index].Target = target;
        _activeAbilities[index].StartCasting(this);
    }

    public void CastAbility(int index)
    {
        _activeAbilities[index].Cast();
    }

    public virtual void TakeDamage(float damage)
    {
        UnitStats.Health -= damage;
        if (UnitStats.Health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    public void Knockback(Vector2 dir, float force = 10)
    {
        _rigidbody2D.AddForce(dir * force, ForceMode2D.Impulse);
    }
}
