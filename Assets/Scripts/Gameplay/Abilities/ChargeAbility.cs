using System.Collections;
using System.Collections.Generic;
using JSAM;
using UnityEngine;

public class ChargeAbility : Ability
{
    [SerializeField] private float _force = 10;
    [SerializeField] private float _damage;
    private Vector2 Dir;
    private Rigidbody2D _rb;
    private float _time = 1.0f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public override void StartCasting(UnitObject unit)
    {
        base.StartCasting(unit);
        Dir = (Target - (Vector2)unit.transform.position).normalized;
        unit.Animator.SetTrigger("Attack Start");
        if (Caster is CrabEnemy)
        {
            AudioManager.PlaySound(Sounds.Crab, transform.position);
        }
        else if (Caster is OrcEnemy)
        {
            AudioManager.PlaySound(Sounds.Orc, transform.position);
        }
    }

    public override void Cast()
    {
        if (!Caster.TryGetComponent(out Rigidbody2D rb))
        {
            Destroy(gameObject);
            return;
        }
        base.Cast();
        Caster.UnitStats.AddLock(0.5f);
        rb.AddForce(Dir * _force, ForceMode2D.Impulse);
    }

    private void Update()
    {
        _time -= Time.deltaTime;
        if (_time <= 0)
        {
            Caster.Animator.SetTrigger("Attack End");
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (Caster == null)
        {
            Destroy(gameObject);
            return;
        }
        _rb.position = Caster.transform.position;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent(out PlayerObject player))
            {
                player.TakeDamage(_damage);
                ContactPoint2D[] contacts = new ContactPoint2D[1];
                collision.GetContacts(contacts);
                player.Knockback(contacts[0].normal);
                Caster.Animator.SetTrigger("Attack End");
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Root"))
        {
            if (!collision.gameObject.TryGetComponent(out Root root)) return;
            root.TakeDamage(1);
        }
    }
}
