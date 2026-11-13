using System;
using UnityEngine;

public class CanDespawner : MonoBehaviour
{
    [SerializeField] private GarbageTruckController _garbageTruckController;
    [SerializeField] private ConveyorManager _conveyorManager;

    private int _failCanCount;
    private int _successCanCount;

    public int FailCount => _failCanCount;
    public int SuccessCount => _successCanCount;
    
    private void OnEnable() => BindInputEvents();
    private void OnDisable() => UnbindInputEvents();

    private void BindInputEvents()
    {
        _garbageTruckController.OnWorkStarted += StartGameCountInit;
    }

    private void UnbindInputEvents()
    {
        _garbageTruckController.OnWorkStarted -= StartGameCountInit;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody == null) return;
        if (collision.gameObject.TryGetComponent<Can>(out _))
        {
            ResultCanCount(collision);
            DespawnCan(collision);
        }
    }

    private void ResultCanCount(Collision collision)
    {
        Can target = collision.transform.GetComponent<Can>();
        
        if (target.IsSuccess)
        {
            _successCanCount++;
        }
        else
        {
            // _garbageTruckController.AddPenalty(1f);
            _failCanCount++;
        }
    }

    private void DespawnCan(Collision collision)
    {
        _conveyorManager.RemoveCan(collision.rigidbody);
        Destroy(collision.gameObject);
    }

    private void StartGameCountInit()
    {
        _failCanCount = 0;
        _successCanCount = 0;
    }
}
