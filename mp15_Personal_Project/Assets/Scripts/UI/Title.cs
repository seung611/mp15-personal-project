using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Title : MonoBehaviour
{
    [SerializeField] private Button _gameStartButton;
    [SerializeField] private Button _helpButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private Button _creditButton;
    [SerializeField] private SceneReference _nextScene;
    
    public void OnClickStart()
    {
        UIManager.Instance.SceneChanger.FadeOut(_nextScene, () =>
        {
            UIManager.Instance.TitleUI.gameObject.SetActive(false);
            UIManager.Instance.InGameUI.gameObject.SetActive(true);
        });
    }

    public void OnClickHelp()
    {
        UIManager.Instance.HelpUI.gameObject.SetActive(true);
    }
    
    public void OnClickCredit()
    {
        UIManager.Instance.CreditUI.gameObject.SetActive(true);
    }
}
