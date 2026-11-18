using UnityEngine;

public class HighSpeedPiston : NodeEffect
{
    public override EffectType EffectType => EffectType.HighSpeedPiston;
    
    public override void SetNodeEffect()
    {
        // 압축기 버튼 쿨타임 감소
        Debug.Log("고속 피스톤 적용");
    }
}
