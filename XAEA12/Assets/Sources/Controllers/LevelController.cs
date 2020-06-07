using System;
using System.Collections;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _laneSize = 5.0f;
    [SerializeField] private float _changeLaneTime = 1.0f;
    
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    // Start is called before the first frame update
    private void Update()
    {
        _transform.position -= _speed * Time.deltaTime * _transform.forward;
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeLaneLeft();
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeLaneRight();
    }

    private void ChangeLaneLeft()
    {
        StartCoroutine(ChangeLane(1.0f));
    }

    private void ChangeLaneRight()
    {
        StartCoroutine(ChangeLane(-1.0f));
    }

    private IEnumerator ChangeLane(float direction)
    {
        Vector3 initialPosition = _transform.position;
        for (float t = 0.0f; t < 1.0f;)
        {
            float deltaPosition = Mathf.Lerp(0.0f, _laneSize, t);
            float deltaTime = Time.deltaTime;

            Vector3 side = (deltaPosition * direction * _transform.right);
            _transform.position = initialPosition + side;

            t += deltaTime / _changeLaneTime;
            yield return null;
        }
    }
}
