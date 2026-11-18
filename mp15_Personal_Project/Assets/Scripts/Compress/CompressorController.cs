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
    [SerializeField] private float _buttonCoolTime = 1f;

    private Camera _camera;
    private Coroutine _runningCoroutine;
    private Animator _anim;
    private float _pressSpeed;
    private bool _isPushButton;
    
    public float ButtonCoolTime { get => _buttonCoolTime; set => _buttonCoolTime = value; }

    private void Awake() => CacheComponents();
    private void OnEnable() => BindInputEvents();
    private void OnDisable() => UnbindInputEvents();

    private void BindInputEvents()
    {
        _pointAction.action.Enable();
        _clickAction.action.Enable();
        _clickAction.action.performed += OnMouseDown;
    }

    private void UnbindInputEvents()
    {
        _pointAction.action.Disable();
        _clickAction.action.Disable();
        _clickAction.action.performed -= OnMouseDown;
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
        if (_runningCoroutine != null) return;
        
        _runningCoroutine = StartCoroutine(PushButtonCoolTime());
    }

    private void DeCompressor()
    {
        _isPushButton = false;
        _runningCoroutine = null;
    }

    private IEnumerator PushButtonCoolTime()
    {
        _isPushButton = true;
        
        while (_isPushButton)
        {
            _pushButtonAnimator.SetTrigger("OnDown");
            _pressedAnimator.SetTrigger("OnDown");
        
            yield return new WaitForSeconds(_buttonCoolTime);

            DeCompressor();
        }
    }

    private void CacheComponents()
    {
        _camera = Camera.main;
    }
}
