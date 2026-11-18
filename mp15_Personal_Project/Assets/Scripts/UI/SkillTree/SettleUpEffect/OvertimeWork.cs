using UnityEngine;

public class OvertimeWork : NodeEffect
{
    public override EffectType EffectType => EffectType.OvertimeWork;

    public override void SetNodeEffect()
    {
        // 작업 시간 증가
        Debug.Log("연장 근무 적용");
        GameManager.Instance.WorkDuration += _node.Node.EffectValue;
    }
}

