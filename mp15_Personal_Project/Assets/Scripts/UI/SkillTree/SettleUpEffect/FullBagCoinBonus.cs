using UnityEngine;

public class FullBagCoinBonus : NodeEffect
{
    public override EffectType EffectType => EffectType.FullBagCoinBonus;

    public override void SetNodeEffect(ApplyEffect applyEffect)
    {
        applyEffect.ApplyFullBagCoinBonus(_node.Node.EffectValue);
    }
}

