using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    // 게임이 시작되면 시간 감소
    // 만약 패널티가 있으면 추가 시간 감소
    // 시간이 다 끝나면 작업 종료 및 캔 생성과 모든 걸 멈추기
    [SerializeField] private float _workDuration = 90f;
    [SerializeField] private InputActionAsset _playerInput;
    [SerializeField] private NodeData _overtimeWorkNode;

    private InputActionMap _playerInputMap;

    private const string OVERTIME_WORK_ID = "Work_1";
    
    private bool _isWorking;
    private bool _isPaused;
    private float _currentTime;
    private float _currentWorkDuration;
    private float _elapsedPenaltyTime;
    private int _lastWarningSecond = -1;

    public bool IsWorking => _isWorking;
    public float BaseWorkDuration => _workDuration;     // 스킬 적용 전 작업 시간 (툴팁 비교용)

    public event Action OnWorkStarted;
    public event Action OnWorkEnded;

    private void Awake()
    {
        SetSingleton();
        _playerInputMap = _playerInput.FindActionMap("Player");
    }
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
        
        if (_currentTime > 0f && _currentTime < 5f)
        {
            // 남은 초가 바뀔 때만 한 번 재생
            int second = Mathf.CeilToInt(_currentTime);
            if (second != _lastWarningSecond)
            {
                _lastWarningSecond = second;
                SoundManager.Instance.PlaySfx(ESfxType.TimeWarning);
            }
        }
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

        _isWorking = true;
        _currentWorkDuration = GetWorkDuration();
        _currentTime = _currentWorkDuration;
        _lastWarningSecond = -1;
        Resume();
        
        SoundManager.Instance.PlayBgm(EBgmType.InGame);
        OnWorkStarted?.Invoke();
    }

    private void OnEndWork()
    {
        if (!_isWorking) return;

        _isWorking = false;
        
        SoundManager.Instance.PlaySfx(ESfxType.WorkEnd);
        SoundManager.Instance.PlayBgm(EBgmType.Result);
        OnWorkEnded?.Invoke();
        UpdateUI();
        
        _elapsedPenaltyTime = 0;
    }

    private float GetWorkDuration()
    {
        if (PlayerManager.Instance.HaveNodes.Contains(OVERTIME_WORK_ID))
        {
            return _workDuration + _overtimeWorkNode.EffectValue;
        }
        else
        {
            return _workDuration;
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        _isPaused = true;
        _playerInputMap.Disable();
    }

    public void Resume()
    {
        Time.timeScale = 1;
        _isPaused = false;
        _playerInputMap.Enable();
    }

    public void LeaveEarly()
    {
        _isWorking = false;
        _elapsedPenaltyTime = 0;
        Resume();
    }

    private void UpdateUI() 
    {
        UIManager.Instance.ResultUI.SetTotalTimeText(_currentWorkDuration - _elapsedPenaltyTime);
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