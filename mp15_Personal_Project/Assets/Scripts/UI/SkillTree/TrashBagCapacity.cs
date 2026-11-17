using UnityEngine;

public class TrashBagCapacity : NodeEffect
{
    public override EffectType EffectType => EffectType.TrashBagCapacity;
    
    public override void SetNodeEffect(ApplyEffect applyEffect)
    {
        applyEffect.AddTrashBagCapacity(_node.Node.EffectValue);
    }
}
