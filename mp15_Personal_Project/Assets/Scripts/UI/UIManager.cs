using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    // 프리팹(PRB_UIManager) 안의 화면들. 프리팹에서는 전부 꺼져 있으므로
    // 어떤 화면을 언제 켤지는 이 스크립트가 정한다.
    [SerializeField] private InputActionReference _pauseButton;
    
    [Header("화면")]
    [SerializeField] public Title TitleUI;                 // [UI] Title
    [SerializeField] public InGame InGameUI;               // [UI] InGame
    [SerializeField] public Result ResultUI;               // [UI] Result
    [SerializeField] public Help HelpUI;                   // [Ui] Help
    [SerializeField] public TitleSetting TitleSettingUI;   // [UI] TitleSetting
    [SerializeField] public InGameSetting InGameSettingUI; // [UI] InGameSetting
    [SerializeField] public Warning WarningUI;             // [UI] Warning
    [SerializeField] public Message MessageUI;             // [UI] Message

    private void Awake() => SetSingleton();
    private void Start() => Init();
    private void OnEnable() => BindInputEvents();
    private void OnDisable() => UnbindInputEvents();

    private void BindInputEvents()
    {
        if (Instance != this) return;
        _pauseButton.action.Enable();
        _pauseButton.action.performed += OnClickEsc;
    }

    private void UnbindInputEvents()
    {
        if (Instance != this) return;
        _pauseButton.action.Disable();
        _pauseButton.action.performed -= OnClickEsc;
    }

    private void OnClickEsc(InputAction.CallbackContext context)
    {
        OnSettingUI();
    }

    public void OnSettingUI()
    {
        if (TitleUI.gameObject.activeSelf)
        {
            TitleSettingUI.gameObject.SetActive(!TitleSettingUI.gameObject.activeSelf); 
        }
        else if (InGameUI.gameObject.activeSelf 
                 && GameManager.Instance.IsWorking)
        {
            InGameSettingUI.gameObject.SetActive(!InGameSettingUI.gameObject.activeSelf);
            
            if (InGameSettingUI.gameObject.activeSelf)
            {
                GameManager.Instance.Pause();
            }
            else
            {
                GameManager.Instance.Resume();
            }
        }
    }
    
    public void Init()
    {
        TitleUI.gameObject.SetActive(true);
        InGameUI.gameObject.SetActive(false);
        ResultUI.gameObject.SetActive(false);
        HelpUI.gameObject.SetActive(false);
        TitleSettingUI.gameObject.SetActive(false);
        InGameSettingUI.gameObject.SetActive(false);
        WarningUI.gameObject.SetActive(false);
        MessageUI.gameObject.SetActive(false);
    }
}
