using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CanSpawner : MonoBehaviour
{
    [SerializeField] private InputActionReference _canSpawn;
    [SerializeField] private GameObject[] _nomalCanPrefab;
    
    private float _spawnInterval = 0.5f;
    private float _beltSpeed;
    private float _elapsed;
    private int _nextIndex;
    
    private void Start() => CacheComponents();
    private void OnEnable() => BindInputEvents();
    private void OnDisable() => UnbindInputEvents();

    private void BindInputEvents()
    {
        _canSpawn.action.Enable();
        _canSpawn.action.performed += OnSpawnCan;
    }

    private void UnbindInputEvents()
    {
        _canSpawn.action.Disable();
        _canSpawn.action.performed -= OnSpawnCan;
    }

    private void OnSpawnCan(InputAction.CallbackContext context)
    {
        
    }

    private IEnumerator CanSpawn()
    {
        yield return null;
    }

    private void CacheComponents()
    {
        
    }
}
