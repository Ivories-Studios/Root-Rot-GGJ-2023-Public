using System.Collections;
using System.Collections.Generic;
using JSAM;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShootAbility : Ability
{
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _projectileDamage;
    private Vector2 Dir;
    private Rigidbody2D _rigidbody;

    public override void StartCasting(UnitObject unit)
    {
        base.StartCasting(unit);
        _rigidbody = GetComponent<Rigidbody2D>();
        unit.Animator.SetTrigger("Attack");
    }

    public override void Cast()
    {
        base.Cast();
        Dir = (Target - (Vector2)Caster.transform.position).normalized;
        _rigidbody.rotation = Vector2.SignedAngle(Vector2.right, Dir) + 180;
        _rigidbody.AddForce(Dir * _projectileSpeed, ForceMode2D.Impulse);
        AudioManager.PlaySound(Sounds.Electricity, transform.position);
    }
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent(out PlayerObject player))
            {
                player.TakeDamage(_projectileDamage);
                ContactPoint2D[] contacts = new ContactPoint2D[1];
                collision.GetContacts(contacts);
                player.Knockback(contacts[0].normal);
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Root"))
        {
            if (!collision.gameObject.TryGetComponent(out Root root)) return;
            root.TakeDamage(1);
        }
        else if (collision.gameObject.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }
}
