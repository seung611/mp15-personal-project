using UnityEngine;

public class SkilledHands : NodeEffect
{
    [SerializeField] private TrashBagController _trashBagController;
    public override EffectType EffectType => EffectType.SkilledHands;
    
    public override void SetNodeEffect()
    {
        // 쓰레기 봉투 직접 교체 시간 -1초
        Debug.Log("능숙한 손 적용");
        _trashBagController.ChangeDuration -= _node.NodeData.EffectValue;
    }
}
