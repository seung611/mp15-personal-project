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
        GameManager.Instance.OnStartWork();
    }

    public void OnClickSkillTreeButton()
    {
        UIManager.Instance.ResultUI.gameObject.SetActive(false);
        // 스킱트리 팝업...
    }
    
    public void TrashBag(int success, int tiedCoin)
    {
        _successCanText.text = $"{success}";
        _tiedCoinText.text = $"{tiedCoin}";
        _coinText.text = $"{_haveCoin}";
    }

    public void SetFailCanCount(int fail)
    {
        _failCanText.text = $"{fail}";
    }

    public void TotalTime(float time)
    {
        int min = (int) (time / 60f);
        int sec = (int) (time % 60f);
        
        _timeText.text = $"{min:00} : {sec:00}";
    }
}
