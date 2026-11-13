using System;
using UnityEngine;

public class GarbageBagController : MonoBehaviour
{
    [SerializeField] private CanDespawner _canDespawner;
    [SerializeField] private int _garbageBagCapacity = 3;
    private int _currentGarbageBagCapacity;
    private int _successCanCount;

    private void OnCollisionEnter(Collision collision)
    {
        Can can = collision.transform.GetComponent<Can>();
        
        if (can == null) return;
        if (!can.IsSuccess) return;

        if (_currentGarbageBagCapacity >= _garbageBagCapacity)
        {
            Debug.Log("넘침");
            _canDespawner.DespawnCan(can);
            return;
        }
        
        _currentGarbageBagCapacity++;
        _successCanCount++;

        if (_currentGarbageBagCapacity == _garbageBagCapacity)
        {
            Debug.Log("가득참");
        }
        
        Debug.Log($"쓰레기통 : {_currentGarbageBagCapacity} / {_garbageBagCapacity}");
        
        _canDespawner.DespawnCan(can);
    }
}
