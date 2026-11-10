using UnityEngine;
using UnityEngine.InputSystem;

public class CompressorController : MonoBehaviour
{
    [SerializeField] private InputActionReference _pressed;
    [SerializeField] private Animator _pressedAnimator;
    // [SerializeField] private Vector3 position;

    private float _pressSpeed;
    
    private void OnEnable() => BindInputEvents();
    private void OnDisable() => UnbindInputEvents();

    private void BindInputEvents()
    {
        _pressed.action.Enable();
        _pressed.action.performed += OnCompressor;
        _pressed.action.canceled += OnDeCompressor;
    }

    private void UnbindInputEvents()
    {
        _pressed.action.Disable();
        _pressed.action.performed -= OnCompressor;
        _pressed.action.canceled -= OnDeCompressor;
    }
    
    private void OnCompressor(InputAction.CallbackContext context)
    {
        _pressedAnimator.SetBool("IsPressed", true);
    }

    private void OnDeCompressor(InputAction.CallbackContext context)
    {
        _pressedAnimator.SetBool("IsPressed", false);
    }
}
