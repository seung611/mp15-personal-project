using System;
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
    [SerializeField] private GameObject[] _guideUI;
    [SerializeField] private Image _changeCoolTime;
    [SerializeField] private Image _pressButtonCoolTime;

    private string _fullText = "가득 참";
    private string _almostText = "거의 참";
    private string _halfText = "절반 참";
    private string _enoughText = "충분함";
    private float _fullPercent = 1f;
    private float _almostPercent = 0.8f;
    private float _halfPercent = 0.5f;

    private int _currentCanCount;
    private bool _isTrashCanView;
    private bool _isTransitioning;

    private int _haveCoin => PlayerManager.Instance.HaveCoin;

    private void Awake() => SetButtonCoolTimerClose();
    
    public void SetTimeUI(float time)
    {
        int min = (int) (time / 60f);
        int sec = (int) (time % 60f);
        
        if (_timeText == null) return;

        if (time < 5f)
        {
            _timeText.text = (time % 60f).ToString("F1");
        }
        else
        {
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
        _currentCanCount = current;
        
        _trashBagText.text = $"{current} / {max}";
        _trashBagSlider.value = ((float)current / max);
        _trashBagState.text = CalculateTrashBagState(current, max);
        
        RefreshTrashBagChangeGuide();
    }

    private string CalculateTrashBagState(int current, int max)
    {
        _currentCanCount = current;
        float trashBagState = (float)current / max;
        if (trashBagState >= _fullPercent)
        {
            return _fullText;
        }

        if (trashBagState >= _almostPercent)
        {
            return _almostText;
        }

        if (trashBagState >= _halfPercent)
        {
            return _halfText;
        }
        
        return _enoughText;
    }

    public void SetGuide(bool isTrashCanView, bool isTransitioning)
    {
        _isTrashCanView = isTrashCanView;
        _isTransitioning = isTransitioning;
        SetPressButtonGuide(isTrashCanView, isTransitioning);
        RefreshTrashBagChangeGuide();
    }

    private void SetPressButtonGuide(bool isTrashCanView, bool isTransitioning)
    {
        if (PlayerManager.Instance.Guide == 0)
        {
            _guideUI[0].SetActive(!isTrashCanView && !isTransitioning);
        }
        else
        {
            _guideUI[0].gameObject.SetActive(false);
        }
    }

    private void RefreshTrashBagChangeGuide()
    {
        _guideUI[1].SetActive(_isTrashCanView && !_isTransitioning && _currentCanCount > 0);
    }

    public void SetChangeCoolTime(float currentTime, float coolTime)
    {
        _changeCoolTime.fillAmount = currentTime / coolTime;
    }

    public void SetButtonCoolTimerOpen()
    {
        _pressButtonCoolTime.gameObject.SetActive(true);
    }

    public void SetButtonCoolTimerClose()
    {
        _pressButtonCoolTime.gameObject.SetActive(false); 
    }

    public void SetPressedButtonCoolTime(float currentTime, float coolTime)
    {
        _pressButtonCoolTime.fillAmount = currentTime / coolTime;
    }
}
