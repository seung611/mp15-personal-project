using UnityEngine;

public class Can : MonoBehaviour
{
    [SerializeField] private GameObject _normalCan;
    [SerializeField] private GameObject _trashCan;
    [SerializeField] private int _canPrice;

    private CompressorContact _piston;
    private bool _isInCompressSpace;

    [SerializeField] private bool _isSuccess;
    
    public bool IsSuccess => _isSuccess;
    public int CanPrice => _canPrice;
    public bool IsInCompressSpace => _isInCompressSpace;

    private void Awake() => Init();
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.attachedRigidbody != null
            && collider.attachedRigidbody.TryGetComponent(out CompressorContact piston))
        {
            _piston = piston;
        }
        else
        {
            _isInCompressSpace = true;
        }
        
        if (IsCanCrush()) CrushCan();
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.attachedRigidbody != null
            && collider.attachedRigidbody.TryGetComponent(out CompressorContact _))
        {
            _piston = null;
        }
        else
        {
            _isInCompressSpace = false;
        }
    }

    private void CrushCan()
    {
        SoundManager.Instance.PlaySfx(ESfxType.Compress);
        _isSuccess = true;
        _normalCan.SetActive(false);
        _trashCan.SetActive(true);
    }
    
    private bool IsCanCrush()
    {
        return !_isSuccess
               && _isInCompressSpace
               && _piston != null
               && _piston.IsPressingDown();
    }

    private void Init()
    {
        _normalCan.SetActive(true);
        _trashCan.SetActive(false);
    }
}
