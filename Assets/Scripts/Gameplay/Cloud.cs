using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private Vector3 _startPosition;
    private bool _isGoingLeft;
    private float _speed;
    
    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
        _speed = Random.Range(0.5f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGoingLeft)
        {
            if (transform.position.x > _startPosition.x - 3)
            {
                transform.Translate(Vector3.left * Time.deltaTime * _speed);
            }
            else
            {
                _isGoingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < _startPosition.x + 3)
            {
                transform.Translate(Vector3.right * Time.deltaTime * _speed);
            }
            else
            {
                _isGoingLeft = true;
            }
        }
    }
}
