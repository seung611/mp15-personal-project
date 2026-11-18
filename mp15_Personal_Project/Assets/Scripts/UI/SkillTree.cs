using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Node[] _node;
    [SerializeField] private GameObject _skillTree;

    private int _haveCoin => PlayerManager.Instance.HaveCoin;
    
    
    private void OnEnable() => BindEvents();
    private void OnDisable() => UnbindEvents();
    
    private void BindEvents()
    {
        GameManager.Instance.OnWorkStarted += SetStartGame;
        UIManager.Instance.ResultUI.OnOpenSkillTree += OpenSkillTree;
        PlayerManager.Instance.OnUpdateSkillTreeUI += UpdateUI;
    }

    private void UnbindEvents()
    {
        GameManager.Instance.OnWorkStarted -= SetStartGame;
        UIManager.Instance.ResultUI.OnOpenSkillTree -= OpenSkillTree;
        PlayerManager.Instance.OnUpdateSkillTreeUI -= UpdateUI;
    }

    private void OpenSkillTree()
    {
        _skillTree.gameObject.SetActive(true);
        UpdateUI();
    }
    
    public void OnClickContinueButton()
    {
        _skillTree.gameObject.SetActive(false);
        UIManager.Instance.ResultUI.gameObject.SetActive(true);
    }

    public void UpdateUI()
    {
        SetCoinsText();
        RefreshAllNodes();
    }
    
    private void SetCoinsText()
    {
        _coinsText.text = $"{_haveCoin}";
    }

    private void RefreshAllNodes()
    {
        foreach (Node node in _node)
        {
            node.Button.RefreshVisual();
        }
    }

    private void SetStartGame()
    {
        _skillTree.gameObject.SetActive(false);
    }
}
