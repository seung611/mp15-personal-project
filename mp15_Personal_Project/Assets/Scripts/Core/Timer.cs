using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private bool _isCounting;
    private float _currentCoolTime;
    private float _skillCoolTime;
    
    public bool IsCounting => _isCounting;
    public float CurrentCoolTime => _currentCoolTime;

    public event Action OnTimeOver;
    
    private void Update()
    {
        if (!_isCounting) return;
        if (_currentCoolTime <= 0)
        {
            _isCounting = false;
            OnTimeOver?.Invoke();
        }

        _currentCoolTime -= Time.deltaTime;
    }

    public void SetTime(float coolTime)
    {
        _isCounting = true;
        _skillCoolTime = coolTime;
        _currentCoolTime = _skillCoolTime;
    }

    public void StopTime()
    {
        _isCounting = false;
    }
}
