using System;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorManager : MonoBehaviour
{
    [SerializeField] private TrashBagController _trashBagController;
    [SerializeField] private Vector3 _localDirection;
    [SerializeField] private List<Rigidbody> _onConveyors; // 벨트 위 오브젝트 목록
    [SerializeField] private float _speed = 1.0f; // 오브젝트 속도

    private Rigidbody _target;

    public Vector3 LocalDirection { get => _localDirection; set => _localDirection = value; }
    
    private void FixedUpdate() => CanMover();
    private void OnEnable() => BindInputEvents();
    private void OnDisable() => UnbindInputEvents();
    
    private void BindInputEvents()
    {
        GameManager.Instance.OnWorkEnded += DespawnAllCans;
    }

    private void UnbindInputEvents()
    {
        if (GameManager.Instance == null) return; 
        GameManager.Instance.OnWorkEnded -= DespawnAllCans;
    }
    
    public void AddCan(Rigidbody target)
    {
        _onConveyors.Add(target);
    }

    public void RemoveCan(Rigidbody target)
    {
        _onConveyors.Remove(target);
    }
    
    private void DespawnAllCans()
    {
        if (GameManager.Instance.IsWorking) return;
        
        _onConveyors.Clear();
    }

    private void CanMover()
    {
        _target = null;
        
        float speed = _speed;
        
        if (!GameManager.Instance.IsWorking) return;
        if (_trashBagController.IsChangeBag)
        {
            speed *= 0.5f;
        }
        
        for (int i = 0; i <= _onConveyors.Count - 1; i++)
        {
            _target = _onConveyors[i];
            
            if (_target == null) continue;
            
            _target.linearVelocity = new Vector3(
                _localDirection.x * speed,
                _target.linearVelocity.y,
                _localDirection.z * speed);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
        }
    }
}
