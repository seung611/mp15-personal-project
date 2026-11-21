using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 타이틀 등에 쓰는 2D 컨베이어 연출.
// 벨트 무늬가 흐르고, 롤러가 돌고, 캔이 벨트 위를 지나간다. (게임 로직과 무관한 장식)
// 일시정지와 상관없이 움직이도록 unscaledDeltaTime을 쓴다.
public class UIConveyor : MonoBehaviour
{
    [Header("벨트")]
    [SerializeField] private RawImage _belt;             // conveyor_belt 텍스처 (Wrap Mode: Repeat)
    [SerializeField] private RectTransform[] _rollers;   // 양 끝 롤러 (선택)
    [SerializeField] private float _speed = 120f;        // 초당 이동 거리 (UI 픽셀)
    [SerializeField] private bool _moveRight = true;

    [Header("캔")]
    [SerializeField] private RectTransform _canArea;     // 캔이 올라갈 영역 (벨트 바로 위, RectMask2D 권장)
    [SerializeField] private Sprite[] _canSprites;       // icon_can_cola / soda / beer 등
    [SerializeField] private float _canSize = 72f;
    [SerializeField] private float _spacing = 150f;      // 캔 사이 기본 간격
    [SerializeField] private float _spacingRandom = 70f; // 간격에 더해지는 무작위 값 (0 ~ 이 값)
    [SerializeField] private float _bumpHeight = 1.5f;   // 벨트 위에서 덜컹거리는 정도

    private readonly List<RectTransform> _cans = new();
    private readonly Stack<RectTransform> _pool = new();
    private float _distanceSinceSpawn;
    private float _nextGap;

    private float Direction => _moveRight ? 1f : -1f;
    private float HalfWidth => _canArea.rect.width * 0.5f;

    private void Start()
    {
        if (_canArea != null && _canSprites != null && _canSprites.Length > 0)
        {
            FillBelt();
        }
    }

    private void Update()
    {
        float move = _speed * Time.unscaledDeltaTime;

        ScrollBelt(move);
        SpinRollers(move);

        if (_canArea == null || _canSprites == null || _canSprites.Length == 0) return;
        MoveCans(move);
        SpawnIfNeeded(move);
    }

    private void ScrollBelt(float move)
    {
        if (_belt == null || _belt.texture == null) return;

        Rect beltRect = _belt.rectTransform.rect;
        float tileWidth = beltRect.height * _belt.texture.width / _belt.texture.height;

        Rect uv = _belt.uvRect;
        uv.width = beltRect.width / tileWidth;
        uv.height = 1f;
        uv.x = Mathf.Repeat(uv.x - Direction * move / tileWidth, 1f);
        _belt.uvRect = uv;
    }

    private void SpinRollers(float move)
    {
        if (_rollers == null) return;

        foreach (RectTransform roller in _rollers)
        {
            if (roller == null) continue;
            float radius = Mathf.Max(roller.rect.width * 0.5f, 1f);
            float angle = -Direction * move / radius * Mathf.Rad2Deg;
            roller.Rotate(0f, 0f, angle);
        }
    }

    private void MoveCans(float move)
    {
        float limit = HalfWidth + _canSize;

        for (int i = _cans.Count - 1; i >= 0; i--)
        {
            RectTransform can = _cans[i];
            Vector2 position = can.anchoredPosition;
            position.x += Direction * move;
            position.y = Mathf.Abs(Mathf.Sin(position.x * 0.06f)) * _bumpHeight;
            can.anchoredPosition = position;

            if (Mathf.Abs(position.x) > limit)
            {
                Recycle(i);
            }
        }
    }

    private void SpawnIfNeeded(float move)
    {
        _distanceSinceSpawn += move;
        if (_distanceSinceSpawn < _nextGap) return;

        Spawn(-Direction * (HalfWidth + _canSize));
    }

    // 시작할 때 벨트 위를 미리 채워 둔다 (빈 벨트에서 시작하지 않게)
    private void FillBelt()
    {
        float x = -Direction * (HalfWidth + _canSize);
        float end = Direction * HalfWidth;

        while ((end - x) * Direction > 0f)
        {
            Spawn(x);
            x += Direction * _nextGap;
        }
        // 맨 처음 놓은 캔이 들어오는 자리에 있으므로, 다음 캔은 그 캔에서 간격만큼 뒤에 나온다
        _distanceSinceSpawn = 0f;
    }

    private void Spawn(float x)
    {
        RectTransform can = _pool.Count > 0 ? _pool.Pop() : CreateCan();
        can.gameObject.SetActive(true);
        can.GetComponent<Image>().sprite = _canSprites[Random.Range(0, _canSprites.Length)];
        can.anchoredPosition = new Vector2(x, 0f);
        _cans.Add(can);

        _distanceSinceSpawn = 0f;
        _nextGap = _spacing + Random.Range(0f, _spacingRandom);
    }

    private RectTransform CreateCan()
    {
        GameObject go = new GameObject("Can", typeof(RectTransform), typeof(Image));
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.SetParent(_canArea, false);
        rect.anchorMin = new Vector2(0.5f, 0f);
        rect.anchorMax = new Vector2(0.5f, 0f);
        rect.pivot = new Vector2(0.5f, 0f);
        rect.sizeDelta = new Vector2(_canSize, _canSize);

        Image image = go.GetComponent<Image>();
        image.preserveAspect = true;
        image.raycastTarget = false;
        return rect;
    }

    private void Recycle(int index)
    {
        RectTransform can = _cans[index];
        _cans.RemoveAt(index);
        can.gameObject.SetActive(false);
        _pool.Push(can);
    }
}
