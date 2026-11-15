using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private InputActionReference _switchCamera;
    [SerializeField] private GarbageTruckController _garbageTruckController;
    [SerializeField] private Camera _camera;
    
    [Header("Camera Position")]
    [SerializeField] private Transform _conveyorView;
    [SerializeField] private Transform _trashCanView;

    private float _trasitionDuration = 0.6f;

    private bool _isTrashCanView;
    private bool _isTransitioning;
    
    public bool IsTrashCanView => _isTrashCanView;
    public bool IsTransitioning => _isTransitioning;
    
    private void OnEnable() => BindInputEvents();
    private void OnDisable() => UnbindInputEvents();
    
    private void BindInputEvents()
    {
        _switchCamera.action.Enable();
        _switchCamera.action.performed += OnSwitchCamera;
        _garbageTruckController.OnWorkStarted += SetCameraPosition;
    }

    private void UnbindInputEvents()
    {
        _switchCamera.action.Disable();
        _switchCamera.action.performed -= OnSwitchCamera;
        _garbageTruckController.OnWorkStarted -= SetCameraPosition;
    }

    private void OnSwitchCamera(InputAction.CallbackContext context)
    {
        if (_isTransitioning) return;
        if (!_garbageTruckController.IsWorking) return;
        
        _isTrashCanView = !_isTrashCanView;
        
        Transform target = _isTrashCanView ? _trashCanView : _conveyorView;
        
        StartCoroutine(MoveCamera(target));
    }

    private IEnumerator MoveCamera(Transform target)
    {
        _isTransitioning = true;
        
        Transform cameraTransform = _camera.transform;
        
        Vector3 startPosition = cameraTransform.position;
        Quaternion startRotation = cameraTransform.rotation;

        float elaspedTime = 0f;

        while (elaspedTime < _trasitionDuration)
        {
            elaspedTime += Time.deltaTime;
            
            float translate = Mathf.Clamp01(elaspedTime / _trasitionDuration);
            
            translate = Mathf.SmoothStep(0f, 1f, translate);
            
            cameraTransform.position = Vector3.Lerp(
                startPosition,
                target.position,
                translate
            );

            cameraTransform.rotation = Quaternion.Slerp(
                startRotation,
                target.rotation,
                translate
            );

            yield return null;
        }

        cameraTransform.SetPositionAndRotation(
            target.position,
            target.rotation
        );

        _isTransitioning = false;
    }

    private void SetCameraPosition()
    {
        _isTrashCanView = false;
        
        _camera.transform.SetPositionAndRotation(
            _conveyorView.position,
            _conveyorView.rotation);
    }
}
