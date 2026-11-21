using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 인게임 가이드 상자 (문구 상자 + 원형 게이지 + 가운데 아이콘).
// 무엇을 보여줄지는 InGame이 정하고, 이 스크립트는 화면에 반영만 한다.
public class GuideBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _box;            // 문구 뒤 상자 이미지 (문구를 숨길 때 같이 숨김)
    [SerializeField] private Image _centerIcon;
    [SerializeField] private Image _gauge;

    public void SetContent(Sprite icon, string message)
    {
        if (_centerIcon != null && icon != null && _centerIcon.sprite != icon)
        {
            _centerIcon.sprite = icon;
        }
        if (_text != null && _text.text != message)
        {
            _text.text = message;
        }
    }
    
    public void SetMessageVisible(bool isVisible)
    {
        if (_text != null) _text.gameObject.SetActive(isVisible);
        if (_box != null) _box.enabled = isVisible;
    }

    public void SetFill(float amount)
    {
        if (_gauge != null) _gauge.fillAmount = amount;
    }
}
