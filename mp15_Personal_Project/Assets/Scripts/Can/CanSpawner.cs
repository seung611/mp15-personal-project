using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class CanSpawner : MonoBehaviour
{
    [SerializeField] private TrashBagController _trashBagController;
    [SerializeField] private GameObject[] _nomalCanPrefab;
    [SerializeField] private SkillTree _skillTree;
    [SerializeField] private Transform _canRoot;
    [SerializeField] private float _canCreateInterval = 5f;
    [SerializeField, Range(0, 100)] private int _percent = 25;

    private const string KEEN_EYE_ID = "Work_2";

    private bool _isFirstTimeKeenEye = true;
    
    private CancellationTokenSource _token = new();
    
    public int Percent { get => _percent; set => _percent = value; }

    private int _basePercent;
    public int BasePercent => _basePercent;                    // 스킬 적용 전 고가 캔 확률 (툴팁 비교용)

    private void Awake() => _basePercent = _percent;

    private void OnEnable() => BindInputEvents();
    private void OnDisable() => UnbindInputEvents();

    private void BindInputEvents()
    {
        GameManager.Instance.OnWorkStarted += OnSpawnCan;
        GameManager.Instance.OnWorkEnded += OnSpawnCanStop;
        UIManager.Instance.WarningUI.OnLeaveEarly += OnSpawnCanStop;
    }

    private void UnbindInputEvents()
    {
        GameManager.Instance.OnWorkStarted -= OnSpawnCan;
        GameManager.Instance.OnWorkEnded -= OnSpawnCanStop;
        UIManager.Instance.WarningUI.OnLeaveEarly -= OnSpawnCanStop;
    }

    // 일반 캔 75%, 고가 캔 25% 랜덤 캔 프리팹
    private GameObject GetRandomPrefab()
    {
        int randomValue = Random.Range(0, 100);

        if (PlayerManager.Instance.HaveNodes.Contains(KEEN_EYE_ID)
            && _isFirstTimeKeenEye)
        {
            _isFirstTimeKeenEye = false;
            
            Node node = _skillTree.NodeDictionary[KEEN_EYE_ID];
            _percent += (int)node.NodeData.EffectValue;
        }

        if (randomValue < _percent)
        {
            return _nomalCanPrefab[0].gameObject;
        }

        int normalIndex = Random.Range(1, _nomalCanPrefab.Length);
        return _nomalCanPrefab[normalIndex].gameObject;
    }
    
    private void OnSpawnCan()
    {
        if (_token != null)
        {
            try
            {
                if (!_token.IsCancellationRequested)
                {
                    _token.Cancel();
                }
            }
            catch (ObjectDisposedException) { /* 이미 폐기 중인 경우 무시 */ }
            
            _token.Dispose();
            _token = null; 
        }
        
        _token = new CancellationTokenSource();
        CanSpawnTimer(_token.Token).Forget();
    }

    private async UniTaskVoid CanSpawnTimer(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            float spawn = _canCreateInterval;
            
            if (_trashBagController.IsChangeBag)
            {
                spawn *= 2f;
            }
            
            CanInstantiate();
            await UniTask.Delay(TimeSpan.FromSeconds(spawn), 
                cancellationToken: token);
        }
    }

    private void CanInstantiate()
    {
        GameObject prefab = GetRandomPrefab();
        GameObject can = Instantiate(prefab, 
            _canRoot.position, 
            Quaternion.Euler(90f, 0f, 0f), 
            _canRoot);
    }

    private void OnSpawnCanStop()
    {
        _token?.Cancel();
        _token?.Dispose();
        _token = null;
        
        DespawnCan();
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
