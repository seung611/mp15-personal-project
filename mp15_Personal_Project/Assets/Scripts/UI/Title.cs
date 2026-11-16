using System;
using UnityEditor;
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
        UIManager.Instance._titleUI.SetActive(false);
    }

    public void OnClickHelp()
    {
        UIManager.Instance._helpUI.SetActive(true);
    }
    
    public void OnClickSetting()
    {
        UIManager.Instance._titleSettingUI.SetActive(true);
    }
}
