using UnityEngine;

public class BigTrashBag : NodeEffect
{
    [SerializeField] private TrashBagController _trashBagController;
    public override EffectType EffectType => EffectType.BigTrashBag;
    
    public override void SetNodeEffect()
    {
        // 쓰레기 봉투 용량 증가
        Debug.Log("큰 봉투 적용");
        _trashBagController.TrashBagCapacity += (int)_node.Node.EffectValue;
    }
}
