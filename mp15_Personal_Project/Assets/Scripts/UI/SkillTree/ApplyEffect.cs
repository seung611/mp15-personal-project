using UnityEngine;

public class ApplyEffect : MonoBehaviour
{
    [SerializeField] private TrashBagController _trashBagController;
    [SerializeField] private CanSpawner _canSpawner;
    [SerializeField] private CanDespawner _canDespawner; 
    
    private void Start() => PlayerManager.Instance.SetApplyEffect(this);

    public void AddBigTrashBag(float value)
    {
        // 쓰레기 봉투 용량 증가
        Debug.Log("큰 봉투 적용");
        _trashBagController.TrashBagCapacity += (int)value;
    }

    public void ApplySkilledHands(float value)
    {
        // 쓰레기 봉투 직접 교체 시간 -1초
        Debug.Log("능숙한 손 적용");
        _trashBagController.ChangeDuration -= value;
    }

    public void ApplyTieBagOnWorkEnd(float value)
    {
        // 작업 종료시 쓰레기통에 있고 봉투로 묶지 않은 캔들도 정산
        Debug.Log("마감 묶기 적용");
        _trashBagController.TiedCanCount += _trashBagController.CurrentCanCount;
    }

    public void ApplyAutoBagChange()
    {
        // 봉투가 가득 차면 자동으로 비워줌
        Debug.Log("자동 교체기 적용");
        
    }
    

    public void ApplyHighSpeedPiston(float value)
    {
        // 압축기 버튼 쿨타임 감소
        Debug.Log("고속 피스톤 적용");
        
    }

    public void ApplyPressureReduceValve(float value)
    {
        // 압축 실패 패널티 시간 감소
        Debug.Log("감압 밸브 적용");
        _canDespawner.PenaltyTime -= value;
    }

    public void ApplyDualCompress(float value)
    {
        // 이중 압축칸... 이건...
        Debug.Log("이중 압축판 적용");
        
    }


    public void ApplyOvertimeWork(float value)
    {
        // 작업 시간 증가
        Debug.Log("연장 근무 적용");
        GameManager.Instance.WorkDuration += value;
    }

    public void ApplyKeenEye(float value)
    {
        // 고가 캔 생성 확률 증가
        Debug.Log("눈썰미 적용");
        _canSpawner.Percent = (int)value;
    }

    public void ApplyFullBagCoinBonus(float value)
    {
        // 봉투를 꽉 채워서 묶으면 보너스 코인
        Debug.Log("꽉 찬 봉투 보너스 적용");
        
    }
}
