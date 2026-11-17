using UnityEngine;

public class HighSpeedPiston : NodeEffect
{
    public override EffectType EffectType => EffectType.HighSpeedPiston;
    
    public override void SetNodeEffect(ApplyEffect applyEffect)
    {
        applyEffect.ApplyHighSpeedPiston(_node.Node.EffectValue);
    }
}
