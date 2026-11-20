using System.Collections.Generic;
using UnityEngine;

public class Can : MonoBehaviour
{
    [SerializeField] private GameObject _normalCan;
    [SerializeField] private GameObject _trashCan;
    [SerializeField] private List<Collider> _triggerList = new();
    [SerializeField] private int _canPrice;
    private float _time = 0f;

    [SerializeField] private bool _isSuccess;
    
    public bool IsSuccess => _isSuccess;
    public int CanPrice => _canPrice;

    private void Awake() => Init();
    private void OnTriggerEnter(Collider collider)
    {
        _triggerList.Add(collider);
        
        if (_triggerList.Count == 2 && IsCanCrush())
        {
            CrushCan();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        _triggerList.Remove(collider);
    }

    private void CrushCan()
    {
        _isSuccess = true;
        _normalCan.SetActive(false);
        _trashCan.SetActive(true);
    }
    
    private bool IsCanCrush()
    {
        foreach (Collider trigger in _triggerList)
        {
            CompressorContact piston = trigger.GetComponentInParent<CompressorContact>();
            if (piston == null) continue;

            return piston.IsPressingDown();
        }

        return false;
    }
    
    public bool IsInCompressSpace
    {
        get
        {
            foreach (Collider trigger in _triggerList)
            {
                if (trigger.GetComponentInParent<CompressorContact>() == null) return true;
            }
            return false;
        }
    }

    private void ChangeToTrashModel()
    {
        _normalCan.SetActive(false);
        _trashCan.SetActive(true);
    }

    private void Init()
    {
        _normalCan.SetActive(true);
        _trashCan.SetActive(false);
    }
}
