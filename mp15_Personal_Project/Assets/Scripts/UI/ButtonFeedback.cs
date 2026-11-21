using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 버튼에 붙이면 마우스 올림 · 누름 효과와 클릭 효과음을 준다.
// 일시정지(Time.timeScale = 0) 중에도 동작하도록 unscaledDeltaTime을 쓴다.
[RequireComponent(typeof(Button))]
public class ButtonFeedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float _hoverScale = 1.05f;
    [SerializeField] private float _pressScale = 0.93f;
    [SerializeField] private float _speed = 18f;
    [SerializeField] private Sprite _pressedSprite;     // 비워 두면 크기 효과만 적용
    [SerializeField] private bool _playClickSound = true;

    private Button _button;
    private Image _image;
    private Sprite _normalSprite;
    private Vector3 _baseScale;
    private float _targetScale = 1f;
    private bool _isHover;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _image = _button.targetGraphic as Image;
        if (_image != null)
        {
            _normalSprite = _image.sprite;
        }
        _baseScale = transform.localScale;
        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        // 팝업이 닫힐 때 눌린 상태가 남지 않게 원래대로
        _isHover = false;
        _targetScale = 1f;
        transform.localScale = _baseScale;
        SetPressedSprite(false);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    private void Update()
    {
        Vector3 target = _baseScale * _targetScale;
        if (transform.localScale == target) return;

        float t = 1f - Mathf.Exp(-_speed * Time.unscaledDeltaTime);
        transform.localScale = Vector3.Lerp(transform.localScale, target, t);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_button.interactable) return;
        _isHover = true;
        _targetScale = _hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHover = false;
        _targetScale = 1f;
        SetPressedSprite(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_button.interactable) return;
        _targetScale = _pressScale;
        SetPressedSprite(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _targetScale = _isHover ? _hoverScale : 1f;
        SetPressedSprite(false);
    }

    private void OnClick()
    {
        if (!_playClickSound || SoundManager.Instance == null) return;
        SoundManager.Instance.PlaySfx(ESfxType.ButtonClick);
    }

    private void SetPressedSprite(bool isPressed)
    {
        if (_image == null || _pressedSprite == null) return;
        _image.sprite = isPressed ? _pressedSprite : _normalSprite;
    }
}
