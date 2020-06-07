using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DestructibleObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other1)
    {
        EffectsController.Instance.PlayExplosion(transform.position);
        Destroy(gameObject);
    }
}
