using System;
using UnityEngine;

public class CompressorContact : MonoBehaviour
{
    [SerializeField] private Animator _pressedAnimator;
    [SerializeField] private BoxCollider _compressCollider;
    [SerializeField] private LayerMask _canLayer;

    private Collider[] _hit = new Collider[4];
    
    private static readonly int PRESS_STATE_HASH = Animator.StringToHash("Press");
    private const float CLIP_FRAMES = 60f;
    private const float PRESS_TOP_Y = 4.5f;
    private const float PRESS_BOTTOM_Y = 3.3f;
    private const float PRESS_IMPACT_TIME = 10f / CLIP_FRAMES;
    private const float PRESS_RISE_START_TIME = 20f / CLIP_FRAMES;
    
    private void FixedUpdate()
    {
        if (!IsPressingDown()) return;
        if (!IsTouchingCan()) return;
        
        ReturnCompressor();
        
    }
    
    public bool IsPressingDown()
    {
        AnimatorStateInfo info = _pressedAnimator.GetCurrentAnimatorStateInfo(0);

        if (info.shortNameHash != PRESS_STATE_HASH) return false;
        if (info.normalizedTime >= PRESS_IMPACT_TIME) return false;

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
            if (can.IsInCompressSpace) continue;

            return true;
        }

        return false;
    }
    
    private void ReturnCompressor()
    {
        float height = transform.localPosition.y;
        float ratio = Mathf.InverseLerp(PRESS_BOTTOM_Y, PRESS_TOP_Y, height);
        float time = Mathf.Lerp(PRESS_RISE_START_TIME, 1f, ratio);
        
        _pressedAnimator.Play(PRESS_STATE_HASH, 0, time);
        SoundManager.Instance.PlaySfx(ESfxType.CompressFail);
    }
}
