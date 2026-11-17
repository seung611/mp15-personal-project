using UnityEngine;

public class BigTrashBag : NodeEffect
{
    public override EffectType EffectType => EffectType.BigTrashBag;
    
    public override void SetNodeEffect(ApplyEffect applyEffect)
    {
        applyEffect.AddBigTrashBag(_node.Node.EffectValue);
    }
}
