using System;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private float _speed = 5.0f;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    // Start is called before the first frame update
    private void Update()
    {
        _transform.position -= _speed * Time.deltaTime * _transform.forward;
    }
}
