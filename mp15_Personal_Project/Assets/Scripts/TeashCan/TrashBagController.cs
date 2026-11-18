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
    [SerializeField] private float _changeDuration = 2f;
    [SerializeField] private int _trashBagCapacity = 3;

    private Coroutine _changeBagCoroutine;
    private bool _isBagFull => _currentCanCount >= _trashBagCapacity;
    private bool _isChangingBag;
    private int _currentCanCount;
    private int _tiedCanCount;
    private int _currentBagCoin;
    private int _tiedCoin;

    public bool IsChangeBag { get => _isChangingBag; set => _isChangingBag = value; }
    public int TrashBagCapacity { get => _trashBagCapacity; set => _trashBagCapacity = value; }
    public float ChangeDuration { get => _changeDuration; set => _changeDuration = value; }
    public int CurrentCanCount { get => _currentCanCount; set => _currentCanCount = value; }
    public int TiedCanCount { get => _tiedCanCount; set => _tiedCanCount = value; }
    
    private void OnEnable() => BindEvents();
    private void OnDisable() => UnbindEvents();
    private void OnCollisionEnter(Collision collision)
    {
        HandleCanArrival(collision);
    }
    
    private void BindEvents()
    {
        GameManager.Instance.OnWorkStarted += ResetForNewWork;
        GameManager.Instance.OnWorkEnded += GameEnd;
        _changeTrashBagAction.action.Enable();
        _stopChangeTrashBagAction.action.Enable();
        _changeTrashBagAction.action.performed += OnChangeBagInput;
        _stopChangeTrashBagAction.action.performed += OnCancelChangeInput;
    }

    private void UnbindEvents()
    {
        GameManager.Instance.OnWorkStarted -= ResetForNewWork;
        GameManager.Instance.OnWorkEnded -= GameEnd;
        _changeTrashBagAction.action.Disable();
        _stopChangeTrashBagAction.action.Disable();
        _changeTrashBagAction.action.performed -= OnChangeBagInput;
        _stopChangeTrashBagAction.action.performed -= OnCancelChangeInput;
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

    private void UpdateInGameUI()
    {
        UIManager.Instance.InGameUI.SetTrashBagUI(_currentCanCount, _trashBagCapacity);
        UIManager.Instance.InGameUI.SetCoinUI(_tiedCoin);
    }

    public void OnChangeBagInput(InputAction.CallbackContext context)
    {
        if (!IsChangeAvailable()) return;
        
        _changeBagCoroutine = StartCoroutine(CorChangeBag());
    }

    private bool IsChangeAvailable()
    {
        return GameManager.Instance.IsWorking 
               && _cameraController.IsTrashCanView 
               && !_cameraController.IsTransitioning 
               && _currentCanCount != 0 
               && _changeBagCoroutine == null;
    }
    
    public IEnumerator CorChangeBag()
    {
        _isChangingBag = true;
        
        yield return new WaitForSeconds(_changeDuration);
        
        CompleteBagChange();
        ResetChangeState();
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
        
        StopCoroutine(_changeBagCoroutine);
        ResetChangeState();
    }

    private void ResetChangeState()
    {
        _changeBagCoroutine = null;
        _isChangingBag = false;
    }

    private void ResetForNewWork()
    {
        _currentCanCount = 0;
        _currentBagCoin = 0;
        _tiedCanCount = 0;
        _tiedCoin = 0;
        UpdateInGameUI();
    }
}
