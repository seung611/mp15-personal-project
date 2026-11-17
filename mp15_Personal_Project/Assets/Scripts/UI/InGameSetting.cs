using UnityEngine;
using UnityEngine.UI;

public class InGameSetting : MonoBehaviour
{
    [SerializeField] private Button _escButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _titleButton;
    [SerializeField] private Slider _musicSlider;
    
    public void OnClickEsc()
    {
        UIManager.Instance.TitleSettingUI.gameObject.SetActive(false);
    }
    
    public void OnClickContinueButton()
    {
        UIManager.Instance.TitleSettingUI.gameObject.SetActive(false);
    }
    
    public void OnClickTitleButton()
    {
        UIManager.Instance.WarningUI.gameObject.SetActive(true);
        UIManager.Instance.TitleSettingUI.gameObject.SetActive(false);
    }

    public void OnMusicValueChange()
    {
        
    }
}
