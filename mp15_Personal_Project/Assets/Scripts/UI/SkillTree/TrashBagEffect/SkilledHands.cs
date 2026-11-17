using UnityEngine;

public class SkilledHands : NodeEffect
{
    public override EffectType EffectType => EffectType.SkilledHands;
    
    public override void SetNodeEffect(ApplyEffect applyEffect)
    {
        applyEffect.ApplySkilledHands(_node.Node.EffectValue);
    }
}
