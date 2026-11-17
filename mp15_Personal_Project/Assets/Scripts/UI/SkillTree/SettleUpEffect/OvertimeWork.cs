using UnityEngine;

public class OvertimeWork : NodeEffect
{
    public override EffectType EffectType => EffectType.OvertimeWork;

    public override void SetNodeEffect(ApplyEffect applyEffect)
    {
        applyEffect.ApplyOvertimeWork(_node.Node.EffectValue);
    }
}

