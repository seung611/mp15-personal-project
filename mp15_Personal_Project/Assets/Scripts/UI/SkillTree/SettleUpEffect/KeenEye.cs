using UnityEngine;

public class KeenEye : NodeEffect
{
    public override EffectType EffectType => EffectType.KeenEye;

    public override void SetNodeEffect(ApplyEffect applyEffect)
    {
        applyEffect.ApplyKeenEye(_node.Node.EffectValue);
    }
}
