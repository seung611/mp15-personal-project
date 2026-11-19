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
    [SerializeField] private SkillTree _skillTree;
    [SerializeField] private Timer _trashBagTimer;
    [SerializeField] private float _changeDuration = 3f;
    [SerializeField] private int _trashBagCapacity = 5;

    private const string BIG_TRASH_BAG_ID = "TrashBag_1";
    private const string SKILLED_HANDS_ID = "TrashBag_2";
    private const string BINDING_OFF_ID = "TrashBag_3";
    private const string AUTO_BAG_CHANGE_ID = "TrashBag_4";
    private const string FULL_BAG_BONUS_ID = "Work_3";

    private bool _isFirstTimeBigTrashBag = true;
    private bool _isFirstTimeSkillHand = true;
    
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
        if (_currentCanCount == _trashBagCapacity 
            && PlayerManager.Instance.HaveNodes.Contains(AUTO_BAG_CHANGE_ID))
        {
            StartChangeBag();
        }
        UpdateInGameUI();
    }

    private void OnChangeBagInput(InputAction.CallbackContext context)
    {
        if (!IsChangeAvailable()) return;
        if (PlayerManager.Instance.HaveNodes.Contains(SKILLED_HANDS_ID) 
            && _isFirstTimeSkillHand)
        {
            _isFirstTimeSkillHand = false;
            Node node = _skillTree.NodeDictionary[SKILLED_HANDS_ID];
            _changeDuration -= node.NodeData.EffectValue;
        }
        
        StartChangeBag();
    }

    private void StartChangeBag()
    {
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
    
    private void ChangeBag()
    {
        FullTrashBag();
        CompleteBagChange();
    }
    
    private void FullTrashBag()
    {
        if (_currentCanCount != _trashBagCapacity) return;
        _fullTrashBagCount++;
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
        if (PlayerManager.Instance.HaveNodes.Contains(BINDING_OFF_ID))
        {
            CompleteBagChange();
        }
        if (PlayerManager.Instance.HaveNodes.Contains(FULL_BAG_BONUS_ID))
        {
            Node node = _skillTree.NodeDictionary[FULL_BAG_BONUS_ID];
            int bonus = _fullTrashBagCount * (int)node.NodeData.EffectValue;
            _tiedCoin += bonus;
        }
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
        if (PlayerManager.Instance.HaveNodes.Contains(BIG_TRASH_BAG_ID) 
            && _isFirstTimeBigTrashBag)
        {
            _isFirstTimeBigTrashBag = false;
            Node node = _skillTree.NodeDictionary[BIG_TRASH_BAG_ID];
            _trashBagCapacity += (int)node.NodeData.EffectValue;
        }
        _currentCanCount = 0;
        _currentBagCoin = 0;
        _tiedCanCount = 0;
        _tiedCoin = 0;
        _fullTrashBagCount = 0;
        UpdateInGameUI();
    }
    
    private void UpdateInGameUI()
    {
        UIManager.Instance.InGameUI.SetTrashBagUI(_currentCanCount, _trashBagCapacity);
        UIManager.Instance.InGameUI.SetCoinUI(_tiedCoin);
    }
}
