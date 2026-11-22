using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// 벽 구멍 + 위험 줄무늬 테두리 + 비닐 커튼을 만든다 (장식 전용, 콜라이더 없음).
// 사용법: 빈 오브젝트에 붙이고 벽 앞에 놓은 뒤, 컴포넌트 ⋮ 메뉴 → "Build Opening".
// 오브젝트의 앞(파란 화살표, +Z)이 카메라 쪽을 보게, 위치는 벽 표면에 맞춰 놓는다.
// 벽에 실제 구멍을 뚫지 않고, 벽 앞으로 _frameDepth만큼 튀어나온 상자 안쪽을 어둡게 칠해 구멍처럼 보이게 한다.
//
// 주의: 콜라이더를 넣지 않는다. Can.cs는 아무 트리거에나 들어가면 압축 공간으로 판단하고,
// 일반 콜라이더는 캔을 막기 때문이다.
public class WallOpening : MonoBehaviour
{
    [Header("Size (m)")]
    [SerializeField] private float _width = 1.2f;
    [SerializeField] private float _height = 0.9f;
    [SerializeField] private float _frameThickness = 0.08f;
    [SerializeField] private float _frameDepth = 0.12f;

    [Header("Curtain")]
    [SerializeField] private int _stripCount = 6;
    [SerializeField] private float _stripGap = 0.012f;
    [SerializeField] private float _stripThickness = 0.01f;
    [SerializeField, Range(0f, 1f)] private float _stripLength = 0.92f;   // 구멍 높이 대비 비율

    [Header("Sway (플레이 중 살랑거림)")]
    [SerializeField] private bool _sway = true;
    [SerializeField] private float _swayAngle = 2.5f;
    [SerializeField] private float _swaySpeed = 1.6f;

    [Header("Materials (비워 두면 Assets/Material에 자동 생성)")]
    [SerializeField] private Material _holeMaterial;
    [SerializeField] private Material _frameMaterial;
    [SerializeField] private Material _stripMaterial;
    [SerializeField] private Texture2D _hazardTexture;

    private const string ROOT_NAME = "_Opening";
    private readonly List<Transform> _pivots = new();
    private readonly List<float> _phases = new();

    private void Awake()
    {
        Transform root = transform.Find(ROOT_NAME);
        if (root == null) return;
        foreach (Transform child in root)
        {
            if (!child.name.StartsWith("StripPivot")) continue;
            _pivots.Add(child);
            _phases.Add(Random.Range(0f, Mathf.PI * 2f));
        }
    }

    private void Update()
    {
        if (!_sway) return;
        float time = Time.time * _swaySpeed;
        for (int i = 0; i < _pivots.Count; i++)
        {
            float angle = Mathf.Sin(time + _phases[i]) * _swayAngle;
            _pivots[i].localRotation = Quaternion.Euler(angle, 0f, 0f);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Build Opening")]
    private void Build()
    {
        EnsureMaterials();

        Transform old = transform.Find(ROOT_NAME);
        if (old != null) Undo.DestroyObjectImmediate(old.gameObject);

        GameObject root = new GameObject(ROOT_NAME);
        Undo.RegisterCreatedObjectUndo(root, "Build Opening");
        root.transform.SetParent(transform, false);

        float w = _width, h = _height, t = _frameThickness, d = _frameDepth;

        // 구멍 안쪽 (어두운 판 + 안쪽 벽 4면)
        CreateBox(root.transform, "Hole_Back", new Vector3(0, 0, 0.005f), new Vector3(w, h, 0.01f), _holeMaterial);
        CreateBox(root.transform, "Hole_Top", new Vector3(0, h / 2, d / 2), new Vector3(w, 0.01f, d), _holeMaterial);
        CreateBox(root.transform, "Hole_Bottom", new Vector3(0, -h / 2, d / 2), new Vector3(w, 0.01f, d), _holeMaterial);
        CreateBox(root.transform, "Hole_Left", new Vector3(-w / 2, 0, d / 2), new Vector3(0.01f, h, d), _holeMaterial);
        CreateBox(root.transform, "Hole_Right", new Vector3(w / 2, 0, d / 2), new Vector3(0.01f, h, d), _holeMaterial);

        // 위험 줄무늬 테두리
        float outerW = w + t * 2;
        CreateFrame(root.transform, "Frame_Top", new Vector3(0, h / 2 + t / 2, d), new Vector3(outerW, t, 0.04f), true);
        CreateFrame(root.transform, "Frame_Bottom", new Vector3(0, -h / 2 - t / 2, d), new Vector3(outerW, t, 0.04f), true);
        CreateFrame(root.transform, "Frame_Left", new Vector3(-w / 2 - t / 2, 0, d), new Vector3(t, h, 0.04f), false);
        CreateFrame(root.transform, "Frame_Right", new Vector3(w / 2 + t / 2, 0, d), new Vector3(t, h, 0.04f), false);

        // 비닐 커튼: 위쪽 축(Pivot)에 매달아서 흔들릴 수 있게
        int n = Mathf.Max(1, _stripCount);
        float stripW = (w - _stripGap * (n + 1)) / n;
        float stripH = h * _stripLength;
        for (int i = 0; i < n; i++)
        {
            float x = -w / 2 + _stripGap + stripW / 2 + i * (stripW + _stripGap);
            GameObject pivot = new GameObject($"StripPivot_{i}");
            pivot.transform.SetParent(root.transform, false);
            pivot.transform.localPosition = new Vector3(x, h / 2, d - 0.03f);
            CreateBox(pivot.transform, "Strip", new Vector3(0, -stripH / 2, 0),
                new Vector3(stripW, stripH, _stripThickness), _stripMaterial);
        }

        EditorUtility.SetDirty(gameObject);
    }

    private GameObject CreateBox(Transform parent, string name, Vector3 pos, Vector3 scale, Material mat)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        DestroyImmediate(go.GetComponent<Collider>());
        go.transform.SetParent(parent, false);
        go.transform.localPosition = pos;
        go.transform.localScale = scale;
        Renderer r = go.GetComponent<Renderer>();
        r.sharedMaterial = mat;
        r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        return go;
    }

    private void CreateFrame(Transform parent, string name, Vector3 pos, Vector3 scale, bool horizontal)
    {
        GameObject go = CreateBox(parent, name, pos, scale, _frameMaterial);
        // 줄무늬가 늘어나지 않게 길이에 맞춰 반복 횟수 지정
        float length = horizontal ? scale.x : scale.y;
        float repeat = Mathf.Max(1f, length / (_frameThickness * 4f));
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetVector("_BaseMap_ST", horizontal ? new Vector4(repeat, 1, 0, 0) : new Vector4(1, repeat, 0, 0));
        go.GetComponent<Renderer>().SetPropertyBlock(block);
    }

    private void EnsureMaterials()
    {
        const string folder = "Assets/Material";
        Shader lit = Shader.Find("Universal Render Pipeline/Lit");
        if (_hazardTexture == null)
            _hazardTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Image/Map/hazard_stripe.png");

        if (_holeMaterial == null)
        {
            _holeMaterial = LoadOrCreate($"{folder}/MAT_OpeningHole.mat", lit, m =>
            {
                m.SetColor("_BaseColor", new Color(0.06f, 0.065f, 0.075f));
                m.SetFloat("_Smoothness", 0.1f);
            });
        }

        if (_frameMaterial == null)
        {
            _frameMaterial = LoadOrCreate($"{folder}/MAT_Hazard.mat", lit, m =>
            {
                if (_hazardTexture != null) m.SetTexture("_BaseMap", _hazardTexture);
                m.SetColor("_BaseColor", Color.white);
                m.SetFloat("_Smoothness", 0.3f);
            });
        }

        if (_stripMaterial == null)
        {
            _stripMaterial = LoadOrCreate($"{folder}/MAT_StripCurtain.mat", lit, m =>
            {
                m.SetColor("_BaseColor", new Color(0.72f, 0.8f, 0.85f, 0.45f));
                m.SetFloat("_Smoothness", 0.8f);
                // Surface Type: Transparent
                m.SetFloat("_Surface", 1f);
                m.SetFloat("_Blend", 0f);
                m.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
                m.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                m.SetFloat("_ZWrite", 0f);
                m.SetFloat("_Cull", 0f);   // 양면
                m.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                m.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            });
        }

        EditorUtility.SetDirty(this);
    }

    private static Material LoadOrCreate(string path, Shader shader, System.Action<Material> setup)
    {
        Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (mat != null) return mat;
        mat = new Material(shader);
        setup(mat);
        AssetDatabase.CreateAsset(mat, path);
        AssetDatabase.SaveAssets();
        return mat;
    }
#endif
}
