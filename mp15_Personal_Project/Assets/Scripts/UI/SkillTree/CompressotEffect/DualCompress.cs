using UnityEngine;

public class DualCompress : NodeEffect
{
    public override EffectType EffectType => EffectType.DualCompress;
    
    public override void SetNodeEffect(ApplyEffect applyEffect)
    {
        applyEffect.ApplyDualCompress(_node.Node.EffectValue);
    }
}
