using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DestructibleObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other1)
    {
        Destroy(gameObject);
    }
}
