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
    
    private bool _isFirstTimeHighSpeedButton = true;
    
    private bool _isPushButton => _compressButtonTimer.IsCounting;
    

    private float _baseButtonCoolTime;
    public float BaseButtonCoolTime => _baseButtonCoolTime;   // 스킬 적용 전 레버 대기 시간 (툴팁 비교용)

    private void Awake()
    {
        CacheComponents();
        _baseButtonCoolTime = _buttonCoolTime;
    }
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
        if (!GameManager.Instance.IsWorking) return;
        if (_isPushButton)
        {
            UIManager.Instance.MessageUI.ShowMessage("버튼 준비 중입니다.");
            return;
        }
        if (PlayerManager.Instance.HaveNodes.Contains(HIGH_SPEED_BUTTON_ID) 
            && _isFirstTimeHighSpeedButton)
        {
            _isFirstTimeHighSpeedButton = false;
            
            Node node = _skillTree.NodeDictionary[HIGH_SPEED_BUTTON_ID];
            _buttonCoolTime -= node.NodeData.EffectValue;
        }
        
        _compressButtonTimer.SetTime(_buttonCoolTime);
        UIManager.Instance.InGameUI.SetButtonCoolTimerOpen();
        _pushButtonAnimator.SetTrigger("OnDown");
        _pressedAnimator.SetTrigger("OnDown");
        SoundManager.Instance.PlaySfx(ESfxType.CompressButton);
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
