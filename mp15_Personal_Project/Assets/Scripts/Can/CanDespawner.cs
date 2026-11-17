using System;
using UnityEngine;

public class CanDespawner : MonoBehaviour
{
    [SerializeField] private ConveyorManager _conveyorManager;
    [SerializeField] private bool _canDeleteSelf;

    private int _failCanCount;
    private int _successCanCount;
    private float _penaltyTime;
    
    public float PenaltyTime { get => _penaltyTime; set => _penaltyTime = value; }

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
            GameManager.Instance.AddPenalty(1f);
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
