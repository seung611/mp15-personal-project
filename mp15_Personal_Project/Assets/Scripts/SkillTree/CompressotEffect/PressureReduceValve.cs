using UnityEngine;

public class PressureReduceValve : NodeEffect
{
    [SerializeField] private CanDespawner _canDespawner;
    public override EffectType EffectType => EffectType.PressureReduceValve;
    
    public override void SetNodeEffect()
    {
        // 압축 실패 패널티 시간 감소
        Debug.Log("감압 밸브 적용");
        _canDespawner.PenaltyTime -= _node.NodeData.EffectValue;
    }
}

