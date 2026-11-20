using System;
using System.Collections.Generic;
using UnityEngine;

public class Can : MonoBehaviour
{
    [SerializeField] private GameObject _normalCan;
    [SerializeField] private GameObject _trashCan;
    [SerializeField] private List<Collider> _triggerList = new();
    [SerializeField] private Timer _canCrushTimer;
    [SerializeField] private int _canPrice;
    private float _time = 0.2f;

    [SerializeField] private bool _isSuccess;
    
    public bool IsSuccess => _isSuccess;
    public int CanPrice => _canPrice;

    private void Awake() => Init();
    private void OnEnable()  => _canCrushTimer.OnTimeOver += ChangeToTrashModel;
    private void OnDisable() => _canCrushTimer.OnTimeOver -= ChangeToTrashModel;
    private void OnTriggerEnter(Collider collider)
    {
        _triggerList.Add(collider);
        
        if (_triggerList.Count == 2 && IsCanCrush())
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
        _canCrushTimer.SetTime(_time);
    }
    
    private bool IsCanCrush()
    {
        foreach (Collider trigger in _triggerList)
        {
            CompressorContact piston = trigger.GetComponentInParent<CompressorContact>();
            if (piston == null) continue;          // 압축 공간 트리거는 넘어감

            return piston.IsPressingDown();        // 피스톤이면 내려가는 중인지 반환
        }

        return false;                              // 피스톤 트리거가 없음
    }

    private void ChangeToTrashModel()
    {
        _normalCan.SetActive(false);
        _trashCan.SetActive(true);
    }

    private void Init()
    {
        _normalCan.SetActive(true);
        _trashCan.SetActive(false);
    }
}
