using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

public class CompressorController : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private InputActionReference _pointAction;
    [SerializeField] private InputActionReference _clickAction;
    [SerializeField] private Animator _pressedAnimator;
    [SerializeField] private Animator _pushButtonAnimator;
    [SerializeField] private SkillTree _skillTree;
    [SerializeField] private Timer _compressButtonTimer;
    [SerializeField] private float _buttonCoolTime = 2f;

    private Camera _camera;
    private Animator _anim;

    private const string HIGH_SPEED_BUTTON_ID = "Comp_1";
    
    private bool _isFirstTimrHighSpeedButton = true;
    
    private bool _isPushButton => _compressButtonTimer.IsCounting;
    
    public float ButtonCoolTime { get => _buttonCoolTime; set => _buttonCoolTime = value; }

    private void Awake() => CacheComponents();
    private void OnEnable() => BindInputEvents();
    private void Update()
    {
        UIManager.Instance.InGameUI.SetPressedButtonCoolTime(
            _compressButtonTimer.CurrentCoolTime, _buttonCoolTime);
    }
    private void OnDisable() => UnbindInputEvents();

    private void BindInputEvents()
    {
        _pointAction.action.Enable();
        _clickAction.action.Enable();
        _clickAction.action.performed += OnMouseDown;
        _compressButtonTimer.OnTimeOver += PushButtonCoolTime;
    }

    private void UnbindInputEvents()
    {
        _pointAction.action.Disable();
        _clickAction.action.Disable();
        _clickAction.action.performed -= OnMouseDown;
        _compressButtonTimer.OnTimeOver -= PushButtonCoolTime;
    }
    
    private void OnMouseDown(InputAction.CallbackContext context)
    {
        GetMousePosition();
    }
    
    private void GetMousePosition()
    {
        Vector2 position = _pointAction.action.ReadValue<Vector2>();
        
        Ray ray = _camera.ScreenPointToRay(position);

        if (Physics.Raycast(ray, Mathf.Infinity, _layerMask))
        {
            CompressorTry();
        }
    }

    private void CompressorTry()
    {
        if (!GameManager.Instance.IsWorking || _isPushButton) return;
        if (PlayerManager.Instance.HaveNodes.Contains(HIGH_SPEED_BUTTON_ID) 
            && _isFirstTimrHighSpeedButton)
        {
            _isFirstTimrHighSpeedButton = false;
            
            Node node = _skillTree.NodeDictionary[HIGH_SPEED_BUTTON_ID];
            _buttonCoolTime -= node.NodeData.EffectValue;
        }
        
        _compressButtonTimer.SetTime(_buttonCoolTime);
        UIManager.Instance.InGameUI.SetButtonCoolTimerOpen();
        _pushButtonAnimator.SetTrigger("OnDown");
        _pressedAnimator.SetTrigger("OnDown");
    }

    private void PushButtonCoolTime()
    {
        UIManager.Instance.InGameUI.SetButtonCoolTimerClose();
    }

    private void CacheComponents()
    {
        _camera = Camera.main;
    }
}
