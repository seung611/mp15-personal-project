public enum NodeState
{
    Purchased,
    Available,
    NotEnoughCoin,
    Hidden
}


public enum EffectType
{
    None,
    // 쓰레기봉투 갈래
    TrashBagCapacity,     // 봉투 용량 증가 (+개)
    TrashBagChangeTime,   // 봉투 교체 시간 감소 (-초)
    TrashBagBonusCoin,    // 봉투를 묶을 때 보너스 코인 (+코인)
    
    // 압축기 갈래
    CompressorWaitTime,   // 압축기 대기 시간 감소 (-초)
    PenaltyReduction,     // 압축 실패 패널티 시간 감소 (-초)
    PenaltyShield,        // 판마다 패널티 1회 무시 (+횟수)

    // 작업 효율 갈래
    WorkTime,             // 작업 시간 증가 (+초)
    CanPrice,             // 캔 하나당 코인 증가 (+코인)
    ConveyorSpeed         // 컨베이어 속도 변화
}