using System.Net;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillNodeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Node _node;
    [SerializeField] private Image _lineToPrev;
    [SerializeField] private SkillToolTip _toolTip;
    [SerializeField] private Button _skillButton;
    [SerializeField] private Image _stateImage;
    [SerializeField] private Color _purchasedColor;
    [SerializeField] private Color _availableColor;
    [SerializeField] private Color _notEnoughCoinColor;

    public NodeData Node => _node.NodeData;

    public void OnClickSkillButton()
    {
        PlayerManager.Instance.TryPurchase(_node);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _toolTip.Show(_node.NodeData, transform as RectTransform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _toolTip.Hide();
    }

    private NodeState GetState()
    {
        if (PlayerManager.Instance.IsPurchased(_node))
        {
            return NodeState.Purchased;
        }
        if (PlayerManager.Instance.CanPurchase(_node))
        {
            return NodeState.Available;
        }
        if (PlayerManager.Instance.IsPrevNodeCleared(_node))
        {
            return NodeState.NotEnoughCoin;
        }
        
        return NodeState.Hidden;
        
    }

    public void RefreshVisual()
    {
        UpdateVisibility();
        
        switch (GetState())
        {
            case NodeState.Available:
                _stateImage.color = _availableColor;
                break;
            case NodeState.NotEnoughCoin:
                _stateImage.color = _notEnoughCoinColor;
                break;
            case NodeState.Purchased:
                _stateImage.color = _purchasedColor;
                break;
            case NodeState.Hidden:
                break;
        }
    }

    private void UpdateVisibility()
    {
        gameObject.SetActive(GetState() != NodeState.Hidden);
        
        if (_lineToPrev == null) return;
        _lineToPrev.gameObject.SetActive(GetState() != NodeState.Hidden);	
    }
}
