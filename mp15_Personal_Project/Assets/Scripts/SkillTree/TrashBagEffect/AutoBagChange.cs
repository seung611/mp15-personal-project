using UnityEngine;

public class AutoBagChange : NodeEffect
{
    [SerializeField] private TrashBagController _trashBagController;
    public override EffectType EffectType => EffectType.AutoBagChange;
    
    public override void SetNodeEffect()
    {
        // 봉투가 가득 차면 자동으로 비워줌
        Debug.Log("자동 교체기 적용");
        if (_trashBagController.CurrentCanCount == _trashBagController.TrashBagCapacity)
        {
            StartCoroutine(_trashBagController.CorChangeBag());
        }
    }
}
