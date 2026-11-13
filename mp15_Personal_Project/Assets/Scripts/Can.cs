using System;
using System.Collections.Generic;
using UnityEngine;

public class Can : MonoBehaviour
{
    [SerializeField] private GameObject _normalCan;
    [SerializeField] private GameObject _trashCan;
    [SerializeField] private List<Collider> _triggerList = new();

    private bool _isSuccess;
    
    public bool IsSuccess => _isSuccess;

    private void Awake() => Init();
    private void OnTriggerEnter(Collider collider)
    {
        _triggerList.Add(collider);
        
        if (_triggerList.Count == 2)
        {
            CrushCan();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        for (int i = 0; i < _triggerList.Count; ++i)
        {
            _triggerList.Remove(collider);
        }
    }

    private void CrushCan()
    {
        _isSuccess = true;
        _normalCan.SetActive(false);
        _trashCan.SetActive(true);
    }

    private void Init()
    {
        _normalCan.SetActive(true);
        _trashCan.SetActive(false);
    }
}
