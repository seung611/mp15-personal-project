using UnityEngine;

public class TieBagOnWorkEnd : NodeEffect
{
    [SerializeField] private TrashBagController _trashBagController;
    public override EffectType EffectType => EffectType.TieBagOnWorkEnd;
    
    public override void SetNodeEffect()
    {
        // 작업 종료시 쓰레기통에 있고 봉투로 묶지 않은 캔들도 정산
        Debug.Log("마감 묶기 적용");
    }
}
