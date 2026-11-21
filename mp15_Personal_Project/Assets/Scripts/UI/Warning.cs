using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Warning : MonoBehaviour
{
    [SerializeField] private GameExit _gameExit;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _discription;
    [SerializeField] private Button _yes;
    [SerializeField] private Button _no;
    [SerializeField] private SceneReference _titleScene;

    [SerializeField] private string _exitGameTitle = "게임을 종료할까요?";
    [SerializeField, TextArea] private string _exitGameDescription = "게임을 끄면 모은 코인과 스킬이 사라져요.";

    [SerializeField] private string _leaveWorkTitle = "타이틀로 나갈까요?";
    [SerializeField, TextArea] private string _leaveWorkDescription = "이번 작업에서 모은 코인은 저장되지 않아요.";

    [SerializeField] private string _leaveResultTitle = "타이틀로 나갈까요?";
    [SerializeField, TextArea] private string _leaveResultDescription = "받은 코인과 스킬은 그대로 남아 있어요.";

    private EWarningType _type;

    public event Action OnLeaveEarly;

    public void SetMessage(EWarningType type)
    {
        _type = type;

        switch (type)
        {
            case EWarningType.ExitGame:
                _title.text = _exitGameTitle;
                _discription.text = _exitGameDescription;
                break;
            case EWarningType.LeaveWork:
                _title.text = _leaveWorkTitle;
                _discription.text = _leaveWorkDescription;
                break;
            case EWarningType.LeaveResult:
                _title.text = _leaveResultTitle;
                _discription.text = _leaveResultDescription;
                break;
        }
    }

    public void OnClickYes()
    {
        if (_type == EWarningType.ExitGame)
        {
            _gameExit.ExitGame();
            return;
        }

        gameObject.SetActive(false);
        GameManager.Instance.LeaveEarly();

        OnLeaveEarly?.Invoke();
        SceneManager.LoadScene(_titleScene.SceneName);
        UIManager.Instance.Init();
        SoundManager.Instance.PlayBgm(EBgmType.Title);
    }

    public void OnClickNo()
    {
        gameObject.SetActive(false);

        if (_type == EWarningType.ExitGame)
        {
            UIManager.Instance.TitleSettingUI.gameObject.SetActive(true);
        }
        else
        {
            UIManager.Instance.InGameSettingUI.gameObject.SetActive(true);
        }
    }
}
