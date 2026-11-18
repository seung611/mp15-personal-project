using UnityEngine;

public class FullBagCoinBonus : NodeEffect
{
    [SerializeField] private TrashBagController _trashBagController;
    public override EffectType EffectType => EffectType.FullBagCoinBonus;

    public override void SetNodeEffect()
    {
        // 봉투를 꽉 채워서 묶으면 보너스 코인
        Debug.Log("꽉 찬 봉투 보너스 적용");
        
    }
}

