using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private bool _isActive = true;
    [SerializeField] private bool _doesDestroyRoots;
    [SerializeField] private int _objectToDestroy;
    [SerializeField] private float _damage;
    private Dictionary<GameObject, float> _invincibilityTime;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_isActive) return;
        if (collision.gameObject.TryGetComponent(out UnitObject unit))
        {
            unit.TakeDamage(_damage);
            unit.Knockback(collision.contacts[0].normal, 20);
            _objectToDestroy--;
            if (_objectToDestroy <= 0)
            {
                //Destroy(gameObject);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isActive || !_doesDestroyRoots) return;
        if (collision.gameObject.CompareTag("Root"))
        {
            if (!collision.gameObject.TryGetComponent(out Root root)) return;
            if (collision.gameObject.transform.root.TryGetComponent(out RootMovementController player))
            {
                player.ClearRoot(root);
            }
        }
    }
}
