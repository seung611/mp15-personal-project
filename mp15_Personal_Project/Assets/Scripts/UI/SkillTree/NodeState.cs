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
    BigTrashBag,          // 큰 봉투: 봉투 용량 증가
    SkilledHands,         // 능숙한 손: 직접 교체 시간 감소
    TieBagOnWorkEnd,      // 마감 묶기: 작업 종료 시 걸려 있던 봉투도 묶어서 정산
    AutoBagChange,        // 자동 교체기: 봉투가 가득 차면 자동 교체
    
    // 압축기 갈래
    HighSpeedPiston,      // 고속 피스톤: 버튼 쿨타임 감소
    PressureReduceValve,  // 감압 밸브: 압축 실패 시 줄어드는 작업 시간 감소
    DualCompress,         // 이중 압축판: 압축 공간 2칸, 버튼 한 번에 공간 안의 캔 전부 압축

    // 정산 갈래
    OvertimeWork,         // 연장 근무: 작업 시간 증가
    KeenEye,              // 눈썰미: 고가 캔 확률 증가
    FullBagCoinBonus      // 꽉 찬 봉투 보너스: 용량을 꽉 채워 묶은 봉투 코인
}