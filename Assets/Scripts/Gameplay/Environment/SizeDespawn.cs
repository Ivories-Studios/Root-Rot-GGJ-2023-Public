using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeDespawn : MonoBehaviour
{
    [SerializeField] private float _time;
    [SerializeField] private AnimationCurve _curve;

    private Vector3 _size;
    private float _timer = 0.0f;

    private void Awake()
    {
        _size = transform.localScale;
        StartCoroutine(Shrink());
    }

    IEnumerator Shrink()
    {
        while (_timer < _time)
        {
            transform.localScale = _size * _curve.Evaluate(_timer / _time);
            _timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        Destroy(gameObject);
    }
}
