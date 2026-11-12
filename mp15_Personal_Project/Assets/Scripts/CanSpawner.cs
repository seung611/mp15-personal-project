using System.Collections;
using UnityEngine;

public class CanSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _nomalCanPrefab;
    [SerializeField] private Transform _canRoot;

    private void Start()
    {
        StartCoroutine(CorCanSpawn());
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
        GameObject prefab = GetRandomPrefab();
        GameObject can = Instantiate(prefab, 
            _canRoot.position, 
            Quaternion.Euler(90f, 0f, 0f), 
            _canRoot);
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
