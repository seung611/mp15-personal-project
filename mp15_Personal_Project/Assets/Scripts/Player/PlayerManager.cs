using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private int _guide;
    private int _haveCoin = 100;
    private HashSet<string> _haveNodes = new();

    public int HaveCoin => _haveCoin;
    public int Guide { get => _guide; set => _guide = value; }

    public HashSet<string> HaveNodes => _haveNodes;

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
        if (!CanPurchase(node))
        {
            SoundManager.Instance.PlaySfx(ESfxType.SkillDenied);
            return false;
        }
        
        _haveNodes.Add(node.NodeData.SkillId);
        SoundManager.Instance.PlaySfx(ESfxType.SkillPurchase);
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
