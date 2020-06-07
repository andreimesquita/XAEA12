﻿using System;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _speed;
    
    private void Awake()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();
        if (_rigidbody == null)
            _rigidbody = GetComponent<Rigidbody>();
        
        OnValidate();
    }

    private void OnValidate()
    {
        if (_rigidbody != null)
            _rigidbody.velocity = transform.forward * _speed;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetCommand("Left");
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SetCommand("Right");
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            SetCommand("Jump");
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            SetCommand("Air");
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            SetCommand("Fire");
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            SetCommand("Bazooka");
        else if (Input.GetKeyDown(KeyCode.Alpha7))
            SetCommand("Explode");
        else if (Input.GetKeyDown(KeyCode.Alpha8))
            SetCommand("Laser");
        else if (Input.GetKeyDown(KeyCode.Alpha9))
            SetCommand("Water");
        else if (Input.GetKeyDown(KeyCode.Alpha0))
            SetCommand("Death");
        else if (Input.GetKeyDown(KeyCode.A))
            SetCommand("Endgame");
    }

    public void SetCommand(string trigger)
    {
        _animator.SetTrigger(trigger);
    }
}
