using System.Collections.Generic;
using System.Linq;
using JSAM;
using UnityEngine;

public class EnemyObject : UnitObject
{
    [SerializeField] protected float AttackRange = 1f;
    [SerializeField] protected float AttackCooldown = 1f;
    [SerializeField] private PlayerUIEvents _playerUIEvents;
    protected float NextAttackTime = 1f;
    protected UnitObject Target;
    [SerializeField] private List<GameObject> _deathParticles = new List<GameObject>();
    
    protected virtual void Start()
    {
        Target = FindObjectOfType<PlayerObject>();
    }
    
    protected override void Update()
    {
        if (GameState.IsPaused || UnitStats.IsDead)
        {
            return;
        }
        base.Update();
        if (UnitStats.IsGrabbed)
        {
            NextAttackTime = 2;
        }
        NextAttackTime -= Time.deltaTime;
        if (Target == null) return;
        if (!(Vector3.Distance(transform.position, Target.transform.position) <= AttackRange)) return;
        if (!(NextAttackTime <= 0)) return;
        if (UnitStats.IsLocked || UnitStats.IsGrabbed) return;
        BeginCastAbility(0, Target.transform.position);
        NextAttackTime = AttackCooldown;
    }

    private void OnEnable()
    {
        _playerUIEvents.OnDead.AddListener(Dance);
    }

    private void OnDisable()
    {
        _playerUIEvents.OnDead.RemoveListener(Dance);
    }

    private void Dance()
    {
        Animator.SetBool("Dance", true);
    }

    protected override void Die()
    {
        base.Die();
        ImpulseSource.GenerateImpulse(2);
        AudioManager.PlaySound(Sounds.Explosions, transform.position);

        Vector3 explosionPos = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, 5);
        foreach (Collider2D c in colliders)
        {
            Rigidbody2D rb = c.GetComponent<Rigidbody2D>();

            if (rb == null) continue;
            rb.AddExplosionForce(10, explosionPos, 5);
        }
        Instantiate(_deathParticles[Random.Range(0, _deathParticles.Count)], transform.position, Quaternion.identity);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

        ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
        collision.GetContacts(contacts);
        float totalImpulse = contacts.Sum(contact => contact.normalImpulse);
        float force = totalImpulse / Time.fixedDeltaTime;
        if (collision.gameObject.CompareTag("Player") && UnitStats.IsGrabbed)
        {
            return;
        }

        if (force > 6000)
        {
            TakeDamage(1);
        }
        else if (force > 4000 && UnitStats.IsGrabbed)
        {
            ImpulseSource.GenerateImpulse(0.5f);
        }
    }
}
