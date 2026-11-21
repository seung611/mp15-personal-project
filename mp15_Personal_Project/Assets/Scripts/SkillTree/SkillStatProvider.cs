using UnityEngine;

// 스킬트리 툴팁에 보여 줄 "스킬 사기 전 값 → 산 뒤 값"을 실제 게임 수치에서 가져온다.
// 스킬트리와 같은 씬에 두고, 각 컨트롤러를 연결한다.
public class SkillStatProvider : MonoBehaviour
{
    [SerializeField] private TrashBagController _trashBag;
    [SerializeField] private CompressorController _compressor;
    [SerializeField] private CanDespawner _failDespawner;
    [SerializeField] private CanSpawner _canSpawner;

    public bool TryGetCompare(NodeData node, out string statName, out string before, out string after)
    {
        float effect = node.EffectValue;
        statName = before = after = null;

        switch (node.EffectType)
        {
            case EffectType.OvertimeWork:
                if (GameManager.Instance == null) return false;
                return Number("작업 시간", "초", GameManager.Instance.BaseWorkDuration, effect, out statName, out before, out after);

            case EffectType.BigTrashBag:
                if (_trashBag == null) return false;
                return Number("봉투 용량", "개", _trashBag.BaseCapacity, effect, out statName, out before, out after);

            case EffectType.SkilledHands:
                if (_trashBag == null) return false;
                return Number("교체 시간", "초", _trashBag.BaseChangeDuration, -effect, out statName, out before, out after);

            case EffectType.HighSpeedPiston:
                if (_compressor == null) return false;
                return Number("레버 대기", "초", _compressor.BaseButtonCoolTime, -effect, out statName, out before, out after);

            case EffectType.PressureReduceValve:
                if (_failDespawner == null) return false;
                return Number("실패 시 시간 감소", "초", _failDespawner.BasePenaltyTime, -effect, out statName, out before, out after);

            case EffectType.KeenEye:
                if (_canSpawner == null) return false;
                return Number("고가 캔 확률", "%", _canSpawner.BasePercent, effect, out statName, out before, out after);

            case EffectType.FullBagCoinBonus:
                return Number("꽉 찬 봉투당 보너스", "코인", 0f, effect, out statName, out before, out after);

            case EffectType.AutoBagChange:
                statName = "자동 교체";
                before = "없음";
                after = _trashBag != null ? $"있음 ({Format(_trashBag.AutoChangeDuration)}초)" : "있음";
                return true;

            case EffectType.TieBagOnWorkEnd:
                statName = "마감 때 걸린 봉투";
                before = "버림";
                after = "정산";
                return true;

            case EffectType.DualCompress:
                statName = "압축 공간";
                before = "1칸";
                after = "2칸";
                return true;

            default:
                return false;
        }
    }

    private static bool Number(string name, string unit, float baseValue, float delta,
        out string statName, out string before, out string after)
    {
        statName = name;
        before = Format(baseValue) + unit;
        after = Format(baseValue + delta) + unit;
        return true;
    }

    private static string Format(float value)
    {
        return Mathf.Approximately(value, Mathf.Round(value)) ? $"{Mathf.RoundToInt(value)}" : $"{value:0.#}";
    }
}
