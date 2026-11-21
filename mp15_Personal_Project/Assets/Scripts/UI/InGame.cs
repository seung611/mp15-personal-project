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
    [SerializeField] private GuideBox _guide;
    [SerializeField] private Sprite _pressIcon;
    [SerializeField] private Sprite _changeIcon;
    [SerializeField, TextArea] private string _pressMessage = "마우스 클릭으로 버튼을 눌러 캔을 압축하세요.";
    [SerializeField, TextArea] private string _changeMessage = "키를 눌러 봉투를 교체하세요.";


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
    private float _pressFill;
    private float _changeFill;

    private int _haveCoin => PlayerManager.Instance.HaveCoin;

    private void Awake() => RefreshGuide();
    
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
        RefreshGuide();
    }

    private void RefreshGuide()
    {
        if (_guide == null) return;
        
        _guide.gameObject.SetActive(!_isTransitioning);
        
        if (_isTransitioning) return;
        if (_isTrashCanView)
        {
            _guide.SetContent(_changeIcon, _changeMessage);
            _guide.SetMessageVisible(_currentCanCount > 0);
            _guide.SetFill(_changeFill);
        }
        else
        {
            bool isNotPressedYet = PlayerManager.Instance != null && PlayerManager.Instance.Guide == 0;
            _guide.SetContent(_pressIcon, _pressMessage);
            _guide.SetMessageVisible(isNotPressedYet);
            _guide.SetFill(_pressFill);
        }
    }

    private void RefreshTrashBagChangeGuide()
    {
        if (_isTrashCanView) RefreshGuide();
    }

    public void SetChangeCoolTime(float currentTime, float coolTime)
    {
        _changeFill = coolTime > 0f ? currentTime / coolTime : 0f;
        if (_isTrashCanView && _guide != null) _guide.SetFill(_changeFill);
    }

    public void SetButtonCoolTimerOpen()
    {
        if (PlayerManager.Instance == null || PlayerManager.Instance.Guide != 0) return;

        PlayerManager.Instance.Guide = 1;
        RefreshGuide();
    }

    public void SetButtonCoolTimerClose()
    {
        _pressFill = 0f;
        if (!_isTrashCanView && _guide != null) _guide.SetFill(_pressFill);
    }

    public void SetPressedButtonCoolTime(float currentTime, float coolTime)
    {
        _pressFill = coolTime > 0f ? currentTime / coolTime : 0f;
        if (!_isTrashCanView && _guide != null) _guide.SetFill(_pressFill);
    }
}
