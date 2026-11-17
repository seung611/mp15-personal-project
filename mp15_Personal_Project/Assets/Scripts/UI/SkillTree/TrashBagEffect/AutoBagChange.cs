using UnityEngine;

public class AutoBagChange : NodeEffect
{
    public override EffectType EffectType => EffectType.AutoBagChange;
    
    public override void SetNodeEffect(ApplyEffect applyEffect)
    {
        applyEffect.ApplyAutoBagChange();
    }
}
