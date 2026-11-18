using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private int _haveCoin = 500;
    private HashSet<string> _haveNodes = new();

    public int HaveCoin => _haveCoin;

    public event Action OnUpdateSkillTreeUI;

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
        return _haveNodes.Contains(node.NodeData.SkillId);
    }
    
    public bool TryPurchase(Node node)
    {
        if (!CanPurchase(node)) return false;
        
        _haveNodes.Add(node.NodeData.SkillId);

        Debug.Log("구매");
        UpdateUI();
        if (node.NodeEffect != null)
        {
            Debug.Log("플레이어 매니저");
            node.NodeEffect.SetNodeEffect();
        }
        UseCoin(node.NodeData.SkillPrice);
        
        return true;
    }

    public bool CanPurchase(Node node)
    {
        return IsPrevNodeCleared(node)
               && !IsPurchased(node)
               && _haveCoin >= node.NodeData.SkillPrice;
    }

    public bool IsPrevNodeCleared(Node node)
    {
        return (node.NodeData.PrevNode == null
                || _haveNodes.Contains(node.NodeData.PrevNode.SkillId));
    }

    private void UpdateUI()
    {
        UIManager.Instance.ResultUI.SetCoinText();
        OnUpdateSkillTreeUI?.Invoke();
    }
}
