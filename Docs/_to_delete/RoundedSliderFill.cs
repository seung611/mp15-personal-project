using UnityEngine;
using UnityEngine.UI;

// 둥근(Sliced) 게이지가 값이 작을 때 찌그러지지 않게 채움 폭을 직접 맞춘다.
// 채움 폭 = 게이지 높이 ~ 전체 폭 사이에서 값에 따라 변함 → 둥근 끝이 항상 온전하게 보임.
// 사용법: Slider의 Fill Rect는 비우고(None), 이 컴포넌트의 Fill 칸에 Fill 오브젝트를 넣는다.
[ExecuteAlways]
[RequireComponent(typeof(Slider))]
public class RoundedSliderFill : MonoBehaviour
{
    [SerializeField] private RectTransform _fill;
    [SerializeField] private bool _hideWhenZero = true;

    private Slider _slider;
    private Image _fillImage;

    private void OnEnable()
    {
        _slider = GetComponent<Slider>();
        _fillImage = _fill != null ? _fill.GetComponent<Image>() : null;
        _slider.onValueChanged.AddListener(OnValueChanged);
        Refresh();
    }

    private void OnDisable()
    {
        _slider.onValueChanged.RemoveListener(OnValueChanged);
    }

    // 편집 화면에서도 Value를 바꾸면 바로 보이게
    private void LateUpdate()
    {
        if (!Application.isPlaying)
        {
            Refresh();
        }
    }

    private void OnRectTransformDimensionsChange() => Refresh();

    private void OnValueChanged(float value) => Refresh();

    private void Refresh()
    {
        if (_fill == null || _slider == null) return;
        if (_fill.parent is not RectTransform area) return;

        float fullWidth = area.rect.width;
        float minWidth = Mathf.Min(area.rect.height, fullWidth);
        float t = Mathf.InverseLerp(_slider.minValue, _slider.maxValue, _slider.value);

        _fill.anchorMin = new Vector2(0f, 0f);
        _fill.anchorMax = new Vector2(0f, 1f);
        _fill.pivot = new Vector2(0f, 0.5f);
        _fill.anchoredPosition = Vector2.zero;
        _fill.sizeDelta = new Vector2(Mathf.Lerp(minWidth, fullWidth, t), 0f);

        if (_fillImage != null)
        {
            _fillImage.enabled = !_hideWhenZero || t > 0f;
        }
    }
}
