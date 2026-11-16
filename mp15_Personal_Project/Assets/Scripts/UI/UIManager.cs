using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    // 프리팹(PRB_UIManager) 안의 화면들. 프리팹에서는 전부 꺼져 있으므로
    // 어떤 화면을 언제 켤지는 이 스크립트가 정한다.
    [Header("화면")]
    [SerializeField] public GameObject _titleUI;     // [UI] Title
    [SerializeField] public GameObject _inGameUI;    // [UI] InGame
    [SerializeField] public GameObject _resultUI;    // [UI] Result
    [SerializeField] public GameObject _helpUI;      // [Ui] Help
    [SerializeField] public GameObject _titleSettingUI;   // [UI] TitleSetting
    [SerializeField] public GameObject _inGameSettingUI;   // [UI] InGameSetting
    [SerializeField] public GameObject _warningUI;   // [UI] Warning

    private void Awake() => SetSingleton();

    private void Start()
    {
        // 게임을 켜면 처음 보는 화면은 타이틀
        _titleUI.SetActive(true);
    }
}
