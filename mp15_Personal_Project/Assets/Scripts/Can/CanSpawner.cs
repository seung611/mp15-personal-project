using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class CanSpawner : MonoBehaviour
{
    [SerializeField] private TrashBagController _trashBagController;
    [SerializeField] private GameObject[] _nomalCanPrefab;
    [SerializeField] private Transform _canRoot;
    [SerializeField] private float _canCreateInterval = 5f;
    
    private Coroutine _runningCoroutine;
    private bool _firstSpawn = true;
    private float _elapsedTime;
    private int _percent = 25;
    
    public int Percent { get => _percent; set => _percent = value; }

    private void OnEnable() => BindInputEvents();
    private void OnDisable() => UnbindInputEvents();

    private void BindInputEvents()
    {
        GameManager.Instance.OnWorkStarted += OnSpawnCan;
        GameManager.Instance.OnWorkEnded += OnSpawnCanStop;
    }

    private void UnbindInputEvents()
    {
        GameManager.Instance.OnWorkStarted -= OnSpawnCan;
        GameManager.Instance.OnWorkEnded -= OnSpawnCanStop;
    }

    // 일반 캔 75%, 고가 캔 25% 랜덤 캔 프리팹
    private GameObject GetRandomPrefab()
    {
        int randomValue = Random.Range(0, 100);

        if (randomValue < _percent)
        {
            return _nomalCanPrefab[0].gameObject;
        }

        int normalIndex = Random.Range(1, 3);
        return _nomalCanPrefab[normalIndex].gameObject;
    }
    
    private void OnSpawnCan()
    {
        if (_runningCoroutine != null) return;
        if (_firstSpawn)
        {
            _firstSpawn = false;
            CanInstantiate();
        }
        
        _runningCoroutine = StartCoroutine(CorCanSpawn());
    }

    private IEnumerator CorCanSpawn()
    {
        while (true)
        {
            float spawnTime = Time.deltaTime;
            
            if (_trashBagController.IsChangeBag)
            {
                spawnTime *= 0.5f;
            }
            
            _elapsedTime += spawnTime;

            if (_elapsedTime > _canCreateInterval)
            {
                CanInstantiate();
            }
            
            yield return null;
        }
    }

    private void CanInstantiate()
    {
        GameObject prefab = GetRandomPrefab();
        GameObject can = Instantiate(prefab, 
            _canRoot.position, 
            Quaternion.Euler(90f, 0f, 0f), 
            _canRoot);
        _elapsedTime = 0f;
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
