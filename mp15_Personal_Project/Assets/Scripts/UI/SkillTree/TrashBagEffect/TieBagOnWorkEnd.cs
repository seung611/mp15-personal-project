using UnityEngine;

public class TieBagOnWorkEnd : NodeEffect
{
    public override EffectType EffectType => EffectType.TieBagOnWorkEnd;
    
    public override void SetNodeEffect(ApplyEffect applyEffect)
    {
        applyEffect.ApplyTieBagOnWorkEnd(_node.Node.EffectValue);
    }
}
