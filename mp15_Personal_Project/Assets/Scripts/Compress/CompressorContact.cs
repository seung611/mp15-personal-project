using System;
using UnityEngine;

public class CompressorContact : MonoBehaviour
{
    [SerializeField] private Animator _pressedAnimator;
    [SerializeField] private BoxCollider _compressCollider;
    [SerializeField] private LayerMask _canLayer;

    private Collider[] _hit = new Collider[4];
    
    private const string PRESS_STATE = "Press";
    private const float PRESS_BOTTOM_TIME = 0.5f;
    
    private void FixedUpdate()
    {
        if (!IsPressingDown()) return;
        if (!IsTouchingCan()) return;
        
        ReturnCompressor();
        
    }
    
    public bool IsPressingDown()
    {
        AnimatorStateInfo info = _pressedAnimator.GetCurrentAnimatorStateInfo(0);

        if (!info.IsName(PRESS_STATE)) return false;
        if (info.normalizedTime >= PRESS_BOTTOM_TIME) return false;

        return true;
    }
    
    private bool IsTouchingCan()
    {
        int count = Physics.OverlapBoxNonAlloc(
            _compressCollider.bounds.center,
            _compressCollider.bounds.extents,
            _hit,
            Quaternion.identity,
            _canLayer);

        for (int i = 0; i < count; i++)
        {
            if (!_hit[i].TryGetComponent(out Can can)) continue;
            if (can.IsSuccess) continue;

            return true;
        }

        return false;
    }
    
    private void ReturnCompressor()
    {
        float progress = _pressedAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        _pressedAnimator.Play(PRESS_STATE, 0, 1f - progress);
    }
}
