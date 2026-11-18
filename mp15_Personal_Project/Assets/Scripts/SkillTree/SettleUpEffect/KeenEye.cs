using UnityEngine;

public class KeenEye : NodeEffect
{
    [SerializeField] private CanSpawner _canSpawner;
    public override EffectType EffectType => EffectType.KeenEye;

    public override void SetNodeEffect()
    {
        // 고가 캔 생성 확률 증가
        Debug.Log("눈썰미 적용");
        _canSpawner.Percent = (int)_node.NodeData.EffectValue;
    }
}
