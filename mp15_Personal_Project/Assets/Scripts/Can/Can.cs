using UnityEngine;

public class Can : MonoBehaviour
{
    [SerializeField] private GameObject _normalCan;
    [SerializeField] private GameObject _trashCan;
    [SerializeField] private int _canPrice;
    [SerializeField] private BoxCollider boxCol;

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
        else if (collider.TryGetComponent(out CompressorSpace space))
        {
            _isInCompressSpace = true;
            Physics.IgnoreCollision(boxCol, space.PlateCollider, true);   // 압축판과 충돌 끄기
        }

        if (IsCanCrush())
        {
            boxCol.size = new Vector3(0.4701921f, 0.4754408f, 0.2793649f);
            boxCol.center = new Vector3(0f, -0.003118142f, 0.1493649f);
            CrushCan();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.attachedRigidbody != null
            && collider.attachedRigidbody.TryGetComponent(out CompressorContact _))
        {
            _piston = null;
        }
        else if (collider.TryGetComponent(out CompressorSpace space))
        {
            _isInCompressSpace = false;

            // 안 찌그러진 캔만 다시 충돌 켜기
            if (!_isSuccess)
                Physics.IgnoreCollision(boxCol, space.PlateCollider, false);
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
