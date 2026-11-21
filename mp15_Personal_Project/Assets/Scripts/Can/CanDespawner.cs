using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CanDespawner : MonoBehaviour
{
    [SerializeField] private ConveyorManager _conveyorManager;
    [SerializeField] private SkillTree _skillTree;
    [SerializeField] private bool _canDeleteSelf;

    private const string PRESSURE_REDUCE_VALVE = "Comp_2";
    
    private bool _isFirstTimePressureReduceValue = true;

    private int _failCanCount;
    private int _successCanCount;
    private float _penaltyTime = 2f;
    
    public float PenaltyTime { get => _penaltyTime; set => _penaltyTime = value; }

    private float _basePenaltyTime;
    public float BasePenaltyTime => _basePenaltyTime;          // 스킬 적용 전 실패 패널티 (툴팁 비교용)

    private void Awake() => _basePenaltyTime = _penaltyTime;

    private void OnEnable() => BindInputEvents();
    private void OnDisable() => UnbindInputEvents();

    private void BindInputEvents()
    {
        GameManager.Instance.OnWorkStarted += StartGameCountInit;
        if (!_canDeleteSelf) return;
        GameManager.Instance.OnWorkEnded += EndGame;
    }

    private void UnbindInputEvents()
    {
        GameManager.Instance.OnWorkStarted -= StartGameCountInit;
        if (!_canDeleteSelf) return;
        GameManager.Instance.OnWorkEnded -= EndGame;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (!_canDeleteSelf) return;
        
        Can target = collision.transform.GetComponent<Can>();
        
        if (target == null) return;
        if (target.gameObject.TryGetComponent<Can>(out _))
        {
            ResultCanCount(target);
            DespawnCan(target);
        }
    }

    private void ResultCanCount(Can can)
    {
        if (can.IsSuccess)
        {
            _successCanCount++;
        }
        else
        {
            if (PlayerManager.Instance.HaveNodes.Contains(PRESSURE_REDUCE_VALVE)
                && _isFirstTimePressureReduceValue)
            {
                _isFirstTimePressureReduceValue = false;
                
                Node node = _skillTree.NodeDictionary[PRESSURE_REDUCE_VALVE];
                _penaltyTime -= node.NodeData.EffectValue;
            }
            
            GameManager.Instance.AddPenalty(_penaltyTime);
            SoundManager.Instance.PlaySfx(ESfxType.TimePenalty);
            _failCanCount++;
        }
    }

    public void DespawnCan(Can can)
    {
        _conveyorManager.RemoveCan(can.GetComponent<Rigidbody>());
        Destroy(can.gameObject);
    }

    private void EndGame()
    {
        UIManager.Instance.ResultUI.SetFailCanText(_failCanCount);
    }

    private void StartGameCountInit()
    {
        _failCanCount = 0;
        _successCanCount = 0;
    }
}
