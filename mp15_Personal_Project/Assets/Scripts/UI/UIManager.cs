using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    // 프리팹(PRB_UIManager) 안의 화면들. 프리팹에서는 전부 꺼져 있으므로
    // 어떤 화면을 언제 켤지는 이 스크립트가 정한다.
    [Header("화면")]
    [SerializeField] public Title TitleUI;     // [UI] Title
    [SerializeField] public InGame InGameUI;    // [UI] InGame
    [SerializeField] public GameObject ResultUI;    // [UI] Result
    [SerializeField] public Help HelpUI;      // [Ui] Help
    [SerializeField] public TitleSetting TitleSettingUI;   // [UI] TitleSetting
    [SerializeField] public GameObject InGameSettingUI;   // [UI] InGameSetting
    [SerializeField] public Warning WarningUI;   // [UI] Warning

    private void Awake() => SetSingleton();

    private void Start()
    {
        // 게임을 켜면 처음 보는 화면은 타이틀
        TitleUI.gameObject.SetActive(true);
    }
}
