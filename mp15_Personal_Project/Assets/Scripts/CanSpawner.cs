using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CanSpawner : MonoBehaviour
{
    [SerializeField] private GarbageTruckController _garbageTruckController;
    [SerializeField] private GarbageBagController _garbageBagController;
    [SerializeField] private GameObject[] _nomalCanPrefab;
    [SerializeField] private Transform _canRoot;
    [SerializeField] private float _canCreateInterval = 2f;
    
    private Coroutine _runningCoroutine;

    private void OnEnable() => BindInputEvents();
    private void OnDisable() => UnbindInputEvents();

    private void BindInputEvents()
    {
        _garbageTruckController.OnWorkStarted += OnSpawnCan;
        _garbageTruckController.OnWorkEnded += OnSpawnCanStop;
    }

    private void UnbindInputEvents()
    {
        _garbageTruckController.OnWorkStarted -= OnSpawnCan;
        _garbageTruckController.OnWorkEnded -= OnSpawnCanStop;
    }

    // 일반 캔 75%, 고가 캔 25% 랜덤 캔 프리팹
    private GameObject GetRandomPrefab()
    {
        int randomValue = Random.Range(0, 100);

        if (randomValue < 25)
        {
            return _nomalCanPrefab[0].gameObject;
        }

        int normalIndex = Random.Range(1, 3);
        return _nomalCanPrefab[normalIndex].gameObject;
    }
    
    private void OnSpawnCan()
    {
        if (_runningCoroutine != null) return;
        
        _runningCoroutine = StartCoroutine(CorCanSpawn());
    }

    private IEnumerator CorCanSpawn()
    {
        
        while (true)
        {
            float canCreateInterval = _canCreateInterval;
            
            GameObject prefab = GetRandomPrefab();
            GameObject can = Instantiate(prefab, 
                _canRoot.position, 
                Quaternion.Euler(90f, 0f, 0f), 
                _canRoot);
            
            if (_garbageBagController.IsTrashBagChange)
            {
                canCreateInterval = _canCreateInterval * 2;
            }
            
            yield return new WaitForSeconds(canCreateInterval);
        }
    }

    private void OnSpawnCanStop()
    {
        StopCoroutine(_runningCoroutine);
        
        DespawnCan();
        _runningCoroutine = null;
    }

    private void DespawnCan()
    {
        for (int i = 0; i <= _canRoot.childCount - 1; i++)
        {
            Transform target = _canRoot.GetChild(i);
            
            if (target == null) continue;
            
            Destroy(target.gameObject);
        }
    }
}
