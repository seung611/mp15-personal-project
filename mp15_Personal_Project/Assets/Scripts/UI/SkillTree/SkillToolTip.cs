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

    private RectTransform _toolTipPosition;

    public void Show(NodeData node, RectTransform target)
    {
        _skillName.text = node.SkillName;
        _description.text = node.Description;
        _nextEffect.text = $"{node.EffectValue}";
        _price.text = $"{node.SkillPrice}";
        
        SetToolTipPosition(target);
        
        gameObject.SetActive(true);
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
