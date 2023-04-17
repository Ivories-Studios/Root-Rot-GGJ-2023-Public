using System.Collections;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    [SerializeField] private float _time;

    private void Awake()
    {
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(_time);
        Destroy(gameObject);
    }
}
