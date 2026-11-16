using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Title : MonoBehaviour
{
    [SerializeField] private Button _gameStartButton;
    [SerializeField] private Button _helpButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private SceneReference _nextScene;
    
    public void OnClickStart()
    {
        SceneManager.LoadScene(_nextScene.SceneName);
        UIManager.Instance.TitleUI.gameObject.SetActive(false);
        UIManager.Instance.InGameUI.gameObject.SetActive(true);
    }

    public void OnClickHelp()
    {
        UIManager.Instance.HelpUI.gameObject.SetActive(true);
    }
    
    public void OnClickSetting()
    {
        UIManager.Instance.TitleSettingUI.gameObject.SetActive(true);
    }
}
