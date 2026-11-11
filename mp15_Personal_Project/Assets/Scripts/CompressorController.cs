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
    [SerializeField] private Animator _pullLeverAnimator;
    [SerializeField] private Animator _pressedAnimator;
    [SerializeField] private Animator _pushButtonAnimator;
    // [SerializeField] private Vector3 position;
    [SerializeField] private float _leverCoolTime = 5f;

    private Camera _camera;
    private Coroutine _runningCoroutine;
    private Animator _anim;
    private float _pressSpeed;
    private bool _isPullLever;

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
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
        {
            Compressor();
        }
    }

    private void Compressor()
    { 
        if (_runningCoroutine != null) return;
        
        _runningCoroutine = StartCoroutine(PullLever());
    }

    private void DeCompressor()
    {
        _isPullLever = false;
        
        // _pullLeverAnimator.SetBool("IsPull", _isPullLever);
        _pushButtonAnimator.SetBool("IsPressed", _isPullLever);
        _pressedAnimator.SetBool("IsPressed", _isPullLever);

        _runningCoroutine = null;
    }

    private IEnumerator PullLever()
    {
        _isPullLever = true;
        
        while (_isPullLever)
        {
            // _pullLeverAnimator.SetBool("IsPull", _isPullLever);
            _pushButtonAnimator.SetBool("IsPressed", _isPullLever);
            _pressedAnimator.SetBool("IsPressed", _isPullLever);
        
            yield return new WaitForSeconds(_leverCoolTime);

            DeCompressor();
        }
    }

    private void CacheComponents()
    {
        _camera = Camera.main;
    }
}
