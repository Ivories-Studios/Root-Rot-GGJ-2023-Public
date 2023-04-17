using UnityEngine;

public class HealthPickupScript : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private GameObject _effect;
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        _animator.SetFloat("Offset", Random.Range(0.0f, 1.0f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent<PlayerObject>(out var t_pl))
        {
            if (t_pl.Team == Teams.Player)
            {
                t_pl.Heal(_health);

                if (_effect != null)
                    Instantiate(_effect, transform.position, Quaternion.identity);

                Destroy(gameObject);
            }
        }
    }
}
