using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class LevelController : MonoBehaviour
{
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _laneSize = 5.0f;
    [SerializeField] private float _changeLaneTime = 1.0f;
    bool b = false;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    // Start is called before the first frame update
    private void Update()
    {
        if (!SimulationController.Instance.Initialized)
            return;

        _transform.position -= _speed * Time.deltaTime * _transform.forward;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeLaneLeft();
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeLaneRight();
    }

    public void ChangeLaneLeft()
    {
        if (!SimulationController.Instance.Initialized)
            return;

        StartCoroutine(ChangeLane(1.0f));
    }

    public void ChangeLaneRight()
    {
        if (!SimulationController.Instance.Initialized)
            return;

        StartCoroutine(ChangeLane(-1.0f));
    }

    private IEnumerator ChangeLane(float direction)
    {
        if (b)
            yield break;

        b = true;

        yield return new WaitForSeconds(_changeLaneTime / 2);

        transform.DOMoveX(transform.position.x + (direction * _laneSize), _changeLaneTime / 2);

        yield return new WaitForSeconds(_changeLaneTime / 2);

        b = false;
    }
}
