using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GarbageBagController : MonoBehaviour
{
    [SerializeField] private InputActionReference _changeTrashBagAction;
    [SerializeField] private InputActionReference _stopChangeTrashBagAction;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private CanDespawner _canDespawner;
    [SerializeField] private int _garbageBagCapacity = 3;
    
    private Coroutine _changeTrashBagCoroutine;
    private bool _isTrashBagChange;
    private float _changeTrashBagTimer = 2f;
    private int _currentGarbageBagCapacity;
    private int _successCanCount;
    private int _bagCanCount;
    private int _canPrice;
    private int _tiedTrashBag;

    public int TiedTrashBag => _tiedTrashBag;
    public bool IsTrashBagChange => _isTrashBagChange;
    
    private void OnEnable() => BindInputAction();

    private void OnDisable() => UnbindInputAction();

    private void OnCollisionEnter(Collision collision)
    {
        asdf(collision);
    }
    
    private void BindInputAction()
    {
        GameManager.Instance.OnWorkStarted += StartGameInit;
        GameManager.Instance.OnWorkEnded += CancelChangeTrashBag;
        _changeTrashBagAction.action.Enable();
        _stopChangeTrashBagAction.action.Enable();
        _changeTrashBagAction.action.performed += ChangeGarbageBag;
        _stopChangeTrashBagAction.action.performed += StopChangeTrashBag;
    }

    private void UnbindInputAction()
    {
        GameManager.Instance.OnWorkStarted -= StartGameInit;
        GameManager.Instance.OnWorkEnded -= CancelChangeTrashBag;
        _changeTrashBagAction.action.Disable();
        _stopChangeTrashBagAction.action.Disable();
        _changeTrashBagAction.action.performed -= ChangeGarbageBag;
        _stopChangeTrashBagAction.action.performed -= StopChangeTrashBag;
    }

    private void asdf(Collision collision) // TODO 이름 나중에 바꾸기...
    {
        Can can = collision.transform.GetComponent<Can>();
        
        if (can == null || !can.IsSuccess) return;

        if (_isTrashBagChange)
        {
            Debug.Log("교체중임");
            _canDespawner.DespawnCan(can);
            return;
        }
        
        if (_currentGarbageBagCapacity >= _garbageBagCapacity)
        {
            Debug.Log("넘침");
            _canDespawner.DespawnCan(can);
            return;
        }

        _canPrice += can.CanPrice;
        _currentGarbageBagCapacity++;
        _successCanCount++;
        Debug.Log($"현재 : {_canPrice} 묶인 쓰레기 봉투 : {_tiedTrashBag }");

        if (_currentGarbageBagCapacity == _garbageBagCapacity)
        {
            Debug.Log("가득참");
        }
        
        Debug.Log($"쓰레기통 : {_currentGarbageBagCapacity} / {_garbageBagCapacity} 쓰레기통 금액 {_canPrice}");
        
        _canDespawner.DespawnCan(can);
    }

    private void ChangeGarbageBag(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.IsWorking || 
            !_cameraController.IsTrashCanView ||
            _cameraController.IsTransitioning ||
            _currentGarbageBagCapacity == 0 ||
            _changeTrashBagCoroutine != null) return;
        
        _changeTrashBagCoroutine = StartCoroutine(ChangeTrashBag());
    }

    private void ChangeComplete()
    {
        _bagCanCount += _currentGarbageBagCapacity;
        _tiedTrashBag += _canPrice;
        _currentGarbageBagCapacity = 0;
        _canPrice = 0;
    }

    private IEnumerator ChangeTrashBag()
    {
        _isTrashBagChange = true;
        Debug.Log("교체 시작 ~~");
        
        yield return new WaitForSeconds(_changeTrashBagTimer);
        
        ChangeComplete();
        
        _changeTrashBagCoroutine = null;
        _isTrashBagChange = false;
        
        Debug.Log("교체 완료");
    }

    private void StopChangeTrashBag(InputAction.CallbackContext context)
    {
        CancelChangeTrashBag();
    }

    private void CancelChangeTrashBag()
    {
        if (!_isTrashBagChange) return;
        
        StopCoroutine(_changeTrashBagCoroutine);
        
        _changeTrashBagCoroutine = null;
        _isTrashBagChange = false;
        
        Debug.Log("교체 취소");
    }

    private void StartGameInit()
    {
        _currentGarbageBagCapacity = 0;
        _successCanCount = 0;
        _bagCanCount = 0;
        _canPrice = 0;
        _tiedTrashBag = 0;
    }
}
