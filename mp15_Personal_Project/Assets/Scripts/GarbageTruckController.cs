using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GarbageTruckController : MonoBehaviour
{
    // 게임이 시작되면 시간 감소
    // 만약 패널티가 있으면 추가 시간 감소
    // 시간이 다 끝나면 작업 종료 및 캔 생성과 모든 걸 멈추기

    [SerializeField] private InputActionReference _gameStartAction;
    [SerializeField] private float _workDuration = 20f;
    [SerializeField] private TextMeshProUGUI _timeText;   // 남은 시간을 보여줄 UI (없어도 동작함)

    private float _currentTime;
    private bool _isWorking;

    public bool IsWorking => _isWorking;

    // 작업이 끝났을 때 다른 스크립트에게 알려주는 이벤트
    public event Action OnWorkStarted;
    public event Action OnWorkEnded;

    private void OnEnable() => BindInputEvents();
    private void OnDisable() => UnbindInputEvents();

    private void Start()
    {
        _currentTime = _workDuration;
        UpdateTimeText();
    }

    private void Update()
    {
        if (!_isWorking) return;

        _currentTime -= Time.deltaTime;   // 매 프레임 흐른 시간만큼 감소
        UpdateTimeText();

        if (_currentTime <= 0f)
        {
            _currentTime = 0f;
            UpdateTimeText();
            EndWork();
        }
    }

    private void BindInputEvents()
    {
        _gameStartAction.action.Enable();
        _gameStartAction.action.started += OnStartWork;
    }

    private void UnbindInputEvents()
    {
        _gameStartAction.action.Disable();
        _gameStartAction.action.started -= OnStartWork;
    }

    private void OnStartWork(InputAction.CallbackContext context)
    {
        if (_isWorking) return;

        _currentTime = _workDuration;   // 시작할 때 시간 초기화
        _isWorking = true;
        
        OnWorkStarted?.Invoke();
    }

    private void EndWork()
    {
        if (!_isWorking) return;

        _isWorking = false;

        OnWorkEnded?.Invoke();
    }

    // 패널티를 받았을 때 호출
    public void AddPenalty(float penaltyTime)
    {
        if (!_isWorking) return;

        _currentTime -= penaltyTime;
    }

    private void UpdateTimeText()
    {
        if (_timeText == null) return;

        if (_currentTime < 5f)
        {
            _timeText.text = (Mathf.Floor(_currentTime * 100f) / 100f).ToString();
        }
        else
        {
            _timeText.text = Mathf.Floor(_currentTime).ToString();
        }
    }
}