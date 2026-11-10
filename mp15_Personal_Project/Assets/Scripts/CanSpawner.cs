using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CanSpawner : MonoBehaviour
{
    [SerializeField] private InputActionReference _canSpawn;
    [SerializeField] private GameObject[] _nomalCanPrefab;
    [SerializeField] private Transform _canRoot;

    private void Start()
    {
        StartCoroutine(CorCanSpawn());
    }
    
    /*private void OnEnable() => BindInputEvents();
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
    }*/
    
    // 일반 캔 75%, 고가 캔 25% 랜덤 캔 프리팹
    private GameObject GetRandomPrefab()
    {
        int randomValue = Random.Range(0, 100);

        if (randomValue < 25)
        {
            return _nomalCanPrefab[0];
        }

        int normalIndex = Random.Range(1, 3);
        return _nomalCanPrefab[normalIndex];
    }
    
    private void OnSpawnCan()
    {
        GameObject prefab = GetRandomPrefab();
        GameObject can = Instantiate(prefab, _canRoot.parent.position, Quaternion.identity, _canRoot);
    }

    private IEnumerator CorCanSpawn()
    {
        while (true)
        {
            OnSpawnCan();   
            yield return new WaitForSeconds(2f);
        }
    }
}
