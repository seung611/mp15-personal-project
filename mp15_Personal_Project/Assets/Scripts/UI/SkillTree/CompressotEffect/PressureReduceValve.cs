using UnityEngine;

public class PressureReduceValve : NodeEffect
{
    public override EffectType EffectType => EffectType.PressureReduceValve;
    
    public override void SetNodeEffect(ApplyEffect applyEffect)
    {
        applyEffect.ApplyPressureReduceValve(_node.Node.EffectValue);
    }
}

