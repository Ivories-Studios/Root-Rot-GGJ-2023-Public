using System.Linq;
using UnityEngine;

public class Destructible : UnitObject
{
    [SerializeField] private float _forceThreshold;
    [SerializeField] private GameObject _fracturedObj;
    [SerializeField] private GameObject _impactObj;
    [SerializeField] private GameObject _explosionObj;
    [SerializeField] private float _camShake;
    [Space]
    [SerializeField] private float _pushRange;
    [SerializeField] private float _pushForce;
    [SerializeField] private int _pushDamage;
    [SerializeField] private LayerMask _pushMask;


    private bool _dead = false;

    private bool impact = false;
    private Vector2 impactPoint = Vector2.zero;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && UnitStats.IsGrabbed)
        {
            return;
        }

        ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
        collision.GetContacts(contacts);
        float totalImpulse = contacts.Sum(contact => contact.normalImpulse);
        float force = totalImpulse / Time.fixedDeltaTime;

        if (force > _forceThreshold)
        {
            impact= true;
            impactPoint = contacts[0].point;

            if (ImpulseSource != null)
            {
                ImpulseSource.GenerateImpulse(_camShake);
            }
            if (_impactObj != null)
            {
                Instantiate(_impactObj, contacts[0].point, 
                    Quaternion.FromToRotation(Vector3.up, contacts[0].normal));
            }

            Collider2D[] t_colls = Physics2D.OverlapCircleAll(transform.position, _pushRange);
            foreach (Collider2D coll in t_colls)
            {
                if (coll.transform.TryGetComponent<Rigidbody2D>(out var t_rb))
                {
                    t_rb.AddExplosionForce(_pushForce, contacts[0].point, _pushRange);
                }
            }

            if (collision.transform.TryGetComponent<Rigidbody2D>(out var t_otherrb))
            {
                foreach (ContactPoint2D c in contacts)
                {
                    t_otherrb.AddForce(c.normalImpulse * c.normal, ForceMode2D.Impulse);
                }
            }

            Die();
        }
    }

    protected override void Die()
    {
        if (_dead) return;
        _dead= true;

        if (_fracturedObj != null)
        {
            GameObject t_frac = Instantiate(_fracturedObj, transform.position, transform.rotation);
            t_frac.transform.localScale = transform.localScale;
            if (_rigidbody2D != null)
            {
                for (int i = 0; i < t_frac.transform.childCount; i++)
                {
                    if (t_frac.transform.GetChild(i).TryGetComponent<Rigidbody2D>(out var t_rb))
                    {
                        t_rb.velocity = _rigidbody2D.velocity;
                    }
                }
            }

            if (impact)
            {
                for (int i = 0; i < t_frac.transform.childCount; i++)
                {
                    if (t_frac.transform.GetChild(i).TryGetComponent<Rigidbody2D>(out var t_rb))
                    {
                        t_rb.AddExplosionForce(_pushForce, impactPoint, _pushRange);
                    }
                }
            }
        }
        if (_explosionObj != null)
        {
            Instantiate(_explosionObj, transform.position, Quaternion.identity);
        }

        if (_pushDamage > 0)
        {
            Collider2D[] t_colls = Physics2D.OverlapCircleAll(transform.position, _pushRange);
            foreach (Collider2D coll in t_colls)
            {
                if (coll.transform != transform && coll.transform.TryGetComponent<UnitObject>(out var t_obj))
                {
                    t_obj.TakeDamage(_pushDamage);
                }
            }
        }

        Destroy(gameObject);
    }
}
