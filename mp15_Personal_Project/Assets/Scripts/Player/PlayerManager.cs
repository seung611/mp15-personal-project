using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private int _haveCoin = 500;
    private HashSet<string> _haveNodes = new HashSet<string>();

    public int HaveCoin => _haveCoin;

    private void Awake() => SetSingleton();

    public void SaveCoin(int roundCoin)
    {
        _haveCoin += roundCoin;
        UpdateUI();
    }

    public void UseCoin(int value)
    {
        if (value > _haveCoin) return;
        _haveCoin -= value;
        UpdateUI();
    }

    public bool IsPurchased(Node node)
    {
        return _haveNodes.Contains(node.SkillId);
    }
    
    public bool TryPurchase(Node node)
    {
        if (!CanPurchase(node)) return false;
        
        _haveNodes.Add(node.SkillId);
        UseCoin(node.SkillPrice);
        
        return true;
    }

    public bool CanPurchase(Node node)
    {
        return IsPrevNodeCleared(node)
               && !IsPurchased(node)
               && _haveCoin >= node.SkillPrice;
    }

    public bool IsPrevNodeCleared(Node node)
    {
        return (node.PrevNode == null
                || _haveNodes.Contains(node.PrevNode.SkillId));
    }


    private void UpdateUI()
    {
        UIManager.Instance.SkillTreeUI.SetCoinsText();
        UIManager.Instance.ResultUI.SetCoinText();
        UIManager.Instance.SkillTreeUI.RefreshAllNodes();
    }
}
