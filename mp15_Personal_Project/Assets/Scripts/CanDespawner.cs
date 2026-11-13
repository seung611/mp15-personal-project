using System;
using UnityEngine;

public class CanDespawner : MonoBehaviour
{
    [SerializeField] private GarbageTruckController _garbageTruckController;
    [SerializeField] private ConveyorManager _conveyorManager;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody == null) return;
        
        if (collision.gameObject.TryGetComponent<Can>(out _))
        {
            DespawnCan(collision);
        }
    }

    private void DespawnCan(Collision collision)
    {
        _conveyorManager.RemoveCan(collision.rigidbody);
        Destroy(collision.gameObject);
    }
}
