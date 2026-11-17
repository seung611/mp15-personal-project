using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private Button _continueButton;
    [SerializeField] private SkillNodeButton[] _nodeButtons;
    
    private int _haveCoin => PlayerManager.Instance.HaveCoin;
    
    public void OnClickContinueButton()
    {
        UIManager.Instance.SkillTreeUI.gameObject.SetActive(false);
        UIManager.Instance.ResultUI.gameObject.SetActive(true);
    }
    
    public void SetCoinsText()
    {
        _coinsText.text = $"{_haveCoin}";
    }

    public void RefreshAllNodes()
    {
        foreach (SkillNodeButton node in _nodeButtons)
        {
            node.RefreshVisual();
        }
    }
}
