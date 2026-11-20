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
    
    private string _titleWarningDescription = "정말로 게임을 나가시겠습니까?";
    private string _inGameWarningDescription = "저장되지 않았습니다. 정말로 타이틀로 나가시겠습니까?";
    
    public event Action OnLeaveEarly;

    public void OnClickYes()
    {
        if (UIManager.Instance.TitleUI.gameObject.activeSelf)
        {
            _gameExit.ExitGame();
        }
        else if (UIManager.Instance.InGameUI.gameObject.activeSelf)
        {
            UIManager.Instance.WarningUI.gameObject.SetActive(false);
            GameManager.Instance.LeaveEarly();
            
            OnLeaveEarly?.Invoke();
            SceneManager.LoadScene(_titleScene.SceneName);
            UIManager.Instance.Init();
            SoundManager.Instance.PlayBgm(EBgmType.Title);
        }
    }

    public void OnClickNo()
    {
        if (UIManager.Instance.TitleUI.gameObject.activeSelf)
        {
            UIManager.Instance.TitleSettingUI.gameObject.SetActive(true);
            UIManager.Instance.WarningUI.gameObject.SetActive(false);
        }
        else if (UIManager.Instance.InGameUI.gameObject.activeSelf)
        {
            UIManager.Instance.InGameSettingUI.gameObject.SetActive(true);
            UIManager.Instance.WarningUI.gameObject.SetActive(false);
        }
    }

    public void SetMessage()
    {
        _title.text = $"[Warning]";
        _discription.text = "Not saved. Are you sure you want to leave?";
    }
}
