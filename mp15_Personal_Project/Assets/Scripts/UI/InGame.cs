using System;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _haveCoinText;
    [SerializeField] private TextMeshProUGUI _getCoinText;
    [SerializeField] private TextMeshProUGUI _trashBagText;
    [SerializeField] private TextMeshProUGUI _trashBagState;
    [SerializeField] private Slider _trashBagSlider;

    private int _haveCoin => PlayerManager.Instance.HaveCoin;
    
    public void SetTimeUI(float time)
    {
        int min = (int) (time / 60f);
        int sec = (int) (time % 60f);
        
        if (_timeText == null) return;

        if (time < 5f)
        {
            //_timeText.text = seconds.ToString(@"ss\.f");
            _timeText.text = (time % 60f).ToString("F1");
        }
        else
        {
            //_timeText.text = minutes.ToString(@"ss") + " : " + seconds.ToString(@"ss");
            _timeText.text = $"{min:00} : {sec:00}";
        }
    }

    public void SetCoinUI(int roundCoin)
    {
        _haveCoinText.text = $"{_haveCoin}";
        _getCoinText.text = $"Get +{roundCoin}";
    }

    public void SetTrashBagUI(int current, int max)
    {
        _trashBagText.text = $"{current} / {max}";
        _trashBagSlider.value = ((float)current / max);
        _trashBagState.text = $"{CalculateTrashBagState(current, max)}";
    }

    private string CalculateTrashBagState(int current, int max)
    {
        /*float num = (float)max / current;
        switch (num)
        {
            case 1 :
                return "충분함";
            case 0.8
        }*/
        return null;
    }
}
