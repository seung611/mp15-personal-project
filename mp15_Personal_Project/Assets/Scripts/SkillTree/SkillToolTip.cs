using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _skillName;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _currentEffect;
    [SerializeField] private TextMeshProUGUI _nextEffect;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private float _offsetY = 3f;

    [SerializeField] private SkillStatProvider _statProvider;
    
    [SerializeField] private Color _beforeColor = new Color(0.6f, 0.65f, 0.78f);
    [SerializeField] private Color _afterColor = new Color(0.36f, 0.79f, 0.29f);

    private RectTransform _toolTipPosition;

    public void Show(NodeData node, RectTransform target)
    {
        _skillName.text = node.SkillName;
        _description.text = node.Description;
        SetCompareText(node);
        _price.text = $"{node.SkillPrice}";
        
        SetToolTipPosition(target);
        
        gameObject.SetActive(true);
    }

    // 사기 전: "교체 시간 5초" → "4초"   /   산 뒤: "적용됨" · "교체 시간 4초"
    private void SetCompareText(NodeData node)
    {
        string statName = null, beforeText = null, afterText = null;
        bool hasCompare = _statProvider != null
                          && _statProvider.TryGetCompare(node, out statName, out beforeText, out afterText);
        _currentEffect.gameObject.SetActive(hasCompare);
        _nextEffect.gameObject.SetActive(hasCompare);
        if (!hasCompare) return;

        string before = ColorUtility.ToHtmlStringRGB(_beforeColor);
        string after = ColorUtility.ToHtmlStringRGB(_afterColor);
        bool isPurchased = PlayerManager.Instance != null && PlayerManager.Instance.HaveNodes.Contains(node.SkillId);

        if (isPurchased)
        {
            _currentEffect.text = $"<color=#{before}>적용됨</color>";
            _nextEffect.text = $"{statName} <color=#{after}>{afterText}</color>";
        }
        else
        {
            _currentEffect.text = $"{statName} <color=#{before}>{beforeText}</color>";
            _nextEffect.text = $"→ <color=#{after}>{afterText}</color>";
        }
    }

    private void SetToolTipPosition(RectTransform target)
    {
        _toolTipPosition = transform as RectTransform;
            
        float up = target.rect.height / 2f + _offsetY;
        
        _toolTipPosition.position = target.position + Vector3.up * up;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
