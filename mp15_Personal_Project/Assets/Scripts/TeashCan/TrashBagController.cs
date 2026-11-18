using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrashBagController : MonoBehaviour
{
    [SerializeField] private InputActionReference _changeTrashBagAction;
    [SerializeField] private InputActionReference _stopChangeTrashBagAction;
    [SerializeField] private PlayerCameraController _cameraController;
    [SerializeField] private CanDespawner _canDespawner;
    [SerializeField] private Timer _trashBagTimer;
    [SerializeField] private float _changeDuration = 5f;
    [SerializeField] private int _trashBagCapacity = 20;

    private bool _isBagFull => _currentCanCount >= _trashBagCapacity;
    private bool _isChangingBag => _trashBagTimer.IsCounting;
    private int _currentCanCount;
    private int _tiedCanCount;
    private int _currentBagCoin;
    private int _tiedCoin;
    private int _fullTrashBagCount;

    public bool IsChangeBag { get => _isChangingBag; }
    public int TrashBagCapacity { get => _trashBagCapacity; set => _trashBagCapacity = value; }
    public float ChangeDuration { get => _changeDuration; set => _changeDuration = value; }
    public int CurrentCanCount { get => _currentCanCount; set => _currentCanCount = value; }
    public int FullTrashBagCount { get => _fullTrashBagCount; set => _fullTrashBagCount = value; }
    public int TiedCoin { get => _tiedCoin; set => _tiedCoin = value; }
    public int CurrentBagCoin { get => _currentBagCoin; set => _currentBagCoin = value; }
    
    private void OnEnable() => BindEvents();
    private void OnCollisionEnter(Collision collision)
    {
        HandleCanArrival(collision);
    }
    private void Update()
    {
        UIManager.Instance.InGameUI.SetChangeCoolTime(_trashBagTimer.CurrentCoolTime, _changeDuration);
    }
    private void OnDisable() => UnbindEvents();
    
    private void BindEvents()
    {
        GameManager.Instance.OnWorkStarted += ResetForNewWork;
        GameManager.Instance.OnWorkEnded += GameEnd;
        _changeTrashBagAction.action.Enable();
        _stopChangeTrashBagAction.action.Enable();
        _changeTrashBagAction.action.performed += OnChangeBagInput;
        _stopChangeTrashBagAction.action.performed += OnCancelChangeInput;
        _trashBagTimer.OnTimeOver += ChangeBag;
    }

    private void UnbindEvents()
    {
        GameManager.Instance.OnWorkStarted -= ResetForNewWork;
        GameManager.Instance.OnWorkEnded -= GameEnd;
        _changeTrashBagAction.action.Disable();
        _stopChangeTrashBagAction.action.Disable();
        _changeTrashBagAction.action.performed -= OnChangeBagInput;
        _stopChangeTrashBagAction.action.performed -= OnCancelChangeInput;
        _trashBagTimer.OnTimeOver -= ChangeBag;
    }

    private void HandleCanArrival(Collision collision)
    {
        Can can = collision.transform.GetComponent<Can>();
        
        if (can == null || !can.IsSuccess) return;

        if (_isChangingBag || _isBagFull)
        {
            DiscardCan(can);
            return;
        }

        AddCanToBag(can);
        UpdateInGameUI();
        
        DiscardCan(can);
    }

    private void DiscardCan(Can can)
    {
        _canDespawner.DespawnCan(can);
    }

    private void AddCanToBag(Can can)
    {
        _currentBagCoin += can.CanPrice;
        _currentCanCount++;
        UpdateInGameUI();
    }

    public void OnChangeBagInput(InputAction.CallbackContext context)
    {
        if (!IsChangeAvailable()) return;
        
        _trashBagTimer.SetTime(_changeDuration);
    }

    private bool IsChangeAvailable()
    {
        return GameManager.Instance.IsWorking 
               && _cameraController.IsTrashCanView 
               && !_cameraController.IsTransitioning 
               && _currentCanCount != 0 
               && !_trashBagTimer.IsCounting;
    }

    private void FullTrashBag()
    {
        if (_currentCanCount != _trashBagCapacity) return;
        _fullTrashBagCount++;
    }
    
    private void ChangeBag()
    {
        FullTrashBag();
        CompleteBagChange();
    }
    
    private void CompleteBagChange()
    {
        _tiedCanCount += _currentCanCount;
        _tiedCoin += _currentBagCoin;
        _currentCanCount = 0;
        _currentBagCoin = 0;
        UpdateInGameUI();
    }

    private void OnCancelChangeInput(InputAction.CallbackContext context)
    {
        CancelBagChange();
    }

    private void GameEnd()
    {
        CancelBagChange();
        PlayerManager.Instance.SaveCoin(_tiedCoin);
        UIManager.Instance.ResultUI.SetTrashBagUI(_tiedCanCount, _tiedCoin);
    }

    private void CancelBagChange()
    {
        if (!_isChangingBag) return;
        
        _trashBagTimer.StopTime();
    }

    private void ResetForNewWork()
    {
        _currentCanCount = 0;
        _currentBagCoin = 0;
        _tiedCanCount = 0;
        _tiedCoin = 0;
        UpdateInGameUI();
    }
    
    private void UpdateInGameUI()
    {
        UIManager.Instance.InGameUI.SetTrashBagUI(_currentCanCount, _trashBagCapacity);
        UIManager.Instance.InGameUI.SetCoinUI(_tiedCoin);
    }
}
