using UnityEngine;

// UI를 위아래로 천천히 둥실거리게 한다. (타이틀 로고 등)
// 켜질 때 위에서 떨어지며 살짝 튕기는 등장 연출도 넣을 수 있다.
// 일시정지(Time.timeScale = 0)와 상관없이 움직이도록 unscaledTime을 쓴다.
[RequireComponent(typeof(RectTransform))]
public class UIFloat : MonoBehaviour
{
    [Header("둥실거림")]
    [SerializeField] private float _height = 6f;        // 위아래로 움직이는 거리
    [SerializeField] private float _speed = 1.2f;       // 클수록 빨리
    [SerializeField] private float _tiltAngle = 0.8f;   // 살짝 기울어지는 각도 (0이면 안 기울어짐)

    [Header("등장 연출")]
    [SerializeField] private bool _dropIn = true;
    [SerializeField] private float _dropHeight = 220f;   // 얼마나 위에서 떨어질지
    [SerializeField] private float _dropDuration = 0.7f;

    private RectTransform _rect;
    private Vector2 _basePosition;
    private float _startTime;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _basePosition = _rect.anchoredPosition;
    }

    private void OnEnable()
    {
        _startTime = Time.unscaledTime;
        if (_rect != null) Apply(0f);
    }

    private void OnDisable()
    {
        // 다른 화면으로 갔다 와도 원래 자리에서 다시 시작
        _rect.anchoredPosition = _basePosition;
        _rect.localRotation = Quaternion.identity;
    }

    private void Update()
    {
        Apply(Time.unscaledTime - _startTime);
    }

    private void Apply(float time)
    {
        float dropOffset = 0f;
        float floatWeight = 1f;

        if (_dropIn && time < _dropDuration)
        {
            float t = time / _dropDuration;
            dropOffset = _dropHeight * (1f - EaseOutBack(t));
            floatWeight = t;                    // 떨어지는 동안에는 둥실거림을 서서히 켬
        }

        float wave = Mathf.Sin(time * _speed * Mathf.PI) * floatWeight;

        _rect.anchoredPosition = _basePosition + new Vector2(0f, wave * _height + dropOffset);
        _rect.localRotation = Quaternion.Euler(0f, 0f, Mathf.Sin(time * _speed * Mathf.PI * 0.5f) * _tiltAngle * floatWeight);
    }

    // 목표 지점을 살짝 지나쳤다가 돌아오는 곡선 (튕기는 느낌)
    private static float EaseOutBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }
}
