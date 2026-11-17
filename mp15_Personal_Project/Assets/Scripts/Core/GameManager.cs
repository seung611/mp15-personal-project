using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    // 게임이 시작되면 시간 감소
    // 만약 패널티가 있으면 추가 시간 감소
    // 시간이 다 끝나면 작업 종료 및 캔 생성과 모든 걸 멈추기
    [SerializeField] private float _workDuration = 60f;
    
    private float _currentTime;
    private bool _isWorking;
    private float _elapsedPenaltyTime;

    public bool IsWorking => _isWorking;
    public float WorkDuration { get => _workDuration; set => _workDuration = value; }

    public event Action OnWorkStarted;
    public event Action OnWorkEnded;

    private void Awake() => SetSingleton();

    private void Start()
    {
        _currentTime = _workDuration;
        UIManager.Instance.InGameUI.SetTimeUI(_currentTime);
    }
    
    private void Update()
    {
        if (!_isWorking) return;

        _currentTime -= Time.deltaTime;   // 매 프레임 흐른 시간만큼 감소
        UIManager.Instance.InGameUI.SetTimeUI(_currentTime);
        
        if (_currentTime <= 0f)
        {
            _currentTime = 0f;
            UIManager.Instance.InGameUI.SetTimeUI(_currentTime);
            OnEndWork();
        }
    }

    public void OnStartWork()
    {
        if (_isWorking) return;

        _currentTime = _workDuration;   // 시작할 때 시간 초기화
        _isWorking = true;
        
        OnWorkStarted?.Invoke();
    }

    private void OnEndWork()
    {
        if (!_isWorking) return;

        _isWorking = false;
        
        OnWorkEnded?.Invoke();
        UpdateUI();
        
        _elapsedPenaltyTime = 0;
    }

    private void UpdateUI() 
    {
        UIManager.Instance.ResultUI.SetTotalTimeText(_workDuration - _elapsedPenaltyTime);
        UIManager.Instance.InGameUI.gameObject.SetActive(false);
        UIManager.Instance.ResultUI.gameObject.SetActive(true);
    }

    // 패널티를 받았을 때 호출
    public void AddPenalty(float penaltyTime)
    {
        if (!_isWorking) return;

        _elapsedPenaltyTime += penaltyTime;
        _currentTime -= penaltyTime;
    }
}