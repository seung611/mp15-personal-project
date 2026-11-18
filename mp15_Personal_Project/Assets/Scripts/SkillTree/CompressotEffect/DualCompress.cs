using UnityEngine;

public class DualCompress : NodeEffect
{
    public override EffectType EffectType => EffectType.DualCompress;
    
    public override void SetNodeEffect()
    {
        // 이중 압축칸... 이건...
        Debug.Log("이중 압축판 적용");
    }
}
