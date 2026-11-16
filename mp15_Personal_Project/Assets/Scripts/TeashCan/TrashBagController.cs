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
    [SerializeField] private int _trashBagCapacity = 3;
    
    private Coroutine _changeBagCoroutine;
    private bool _isChangingBag;
    private bool _isBagFull => _currentCanCount >= _trashBagCapacity;
    private float _changeDuration = 2f;
    private int _currentCanCount;
    private int _processedCanCount;
    private int _tiedCanCount;
    private int _currentBagCoin;
    private int _tiedCoin;

    public bool IsChangeBag => _isChangingBag;
    public int TiedCoin => _tiedCoin;
    
    private void OnEnable() => BindEvents();
    private void OnDisable() => UnbindEvents();
    private void OnCollisionEnter(Collision collision)
    {
        HandleCanArrival(collision);
    }
    
    private void BindEvents()
    {
        GameManager.Instance.OnWorkStarted += ResetForNewWork;
        GameManager.Instance.OnWorkEnded += CancelBagChange;
        _changeTrashBagAction.action.Enable();
        _stopChangeTrashBagAction.action.Enable();
        _changeTrashBagAction.action.performed += OnChangeBagInput;
        _stopChangeTrashBagAction.action.performed += OnCancelChangeInput;
    }

    private void UnbindEvents()
    {
        GameManager.Instance.OnWorkStarted -= ResetForNewWork;
        GameManager.Instance.OnWorkEnded -= CancelBagChange;
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
        UpdateBagUI();
        
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
        _processedCanCount++;
    }

    private void UpdateBagUI()
    {
        UIManager.Instance.InGameUI.SetTrashBagUI(_currentCanCount, _trashBagCapacity);
    }

    private void OnChangeBagInput(InputAction.CallbackContext context)
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
    
    private IEnumerator CorChangeBag()
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
        UpdateBagUI();
    }

    private void OnCancelChangeInput(InputAction.CallbackContext context)
    {
        CancelBagChange();
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
        _processedCanCount = 0;
        _tiedCanCount = 0;
        _currentBagCoin = 0;
        _tiedCoin = 0;
        UpdateBagUI();
    }
}
