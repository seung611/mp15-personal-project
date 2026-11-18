using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    // 프리팹(PRB_UIManager) 안의 화면들. 프리팹에서는 전부 꺼져 있으므로
    // 어떤 화면을 언제 켤지는 이 스크립트가 정한다.
    [Header("화면")]
    [SerializeField] public Title TitleUI;                 // [UI] Title
    [SerializeField] public InGame InGameUI;               // [UI] InGame
    [SerializeField] public Result ResultUI;               // [UI] Result
    [SerializeField] public Help HelpUI;                   // [Ui] Help
    [SerializeField] public TitleSetting TitleSettingUI;   // [UI] TitleSetting
    [SerializeField] public InGameSetting InGameSettingUI; // [UI] InGameSetting
    [SerializeField] public Warning WarningUI;             // [UI] Warning

    private void Awake() => SetSingleton();

    private void Start() => Init();

    private void Init()
    {
        TitleUI.gameObject.SetActive(true);
        InGameUI.gameObject.SetActive(false);
        ResultUI.gameObject.SetActive(false);
        HelpUI.gameObject.SetActive(false);
        TitleSettingUI.gameObject.SetActive(false);
        InGameSettingUI.gameObject.SetActive(false);
        WarningUI.gameObject.SetActive(false);
    }
}
