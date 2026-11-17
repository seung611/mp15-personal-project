using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] private Button _nextWorkButton;
    [SerializeField] private Button _skillTreeButton;
    [SerializeField] private TextMeshProUGUI _successCanText;
    [SerializeField] private TextMeshProUGUI _failCanText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _tiedCoinText;
    [SerializeField] private TextMeshProUGUI _coinText;

    private int _haveCoin => PlayerManager.Instance.HaveCoin;

    public void OnClickNextWorkButton()
    {
        UIManager.Instance.ResultUI.gameObject.SetActive(false);
        UIManager.Instance.InGameUI.gameObject.SetActive(true);
        GameManager.Instance.OnStartWork();
    }

    public void OnClickSkillTreeButton()
    {
        UIManager.Instance.ResultUI.gameObject.SetActive(false);
        UIManager.Instance.SkillTreeUI.gameObject.SetActive(true);
        UIManager.Instance.SkillTreeUI.SetCoinsText();
        UIManager.Instance.SkillTreeUI.RefreshAllNodes();
    }
    
    public void SetTrashBagUI(int success, int tiedCoin)
    {
        _successCanText.text = $"{success}";
        _tiedCoinText.text = $"{tiedCoin}";
    }

    public void SetCoinText()
    {
        _coinText.text = $"{_haveCoin}";
    }

    public void SetFailCanText(int fail)
    {
        _failCanText.text = $"{fail}";
    }

    public void SetTotalTimeText(float time)
    {
        int min = (int) (time / 60f);
        int sec = (int) (time % 60f);
        
        _timeText.text = $"{min:00} : {sec:00}";
    }
}
