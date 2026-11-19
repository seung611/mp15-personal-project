using UnityEngine;
using UnityEngine.UI;

public class InGameSetting : MonoBehaviour
{
    [SerializeField] private Button _escButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _titleButton;
    [SerializeField] private Slider _musicSlider;
    
    public void OnClickContinueButton()
    {
        UIManager.Instance.OnSettingUI();
    }
    
    public void OnClickTitleButton()
    {
        UIManager.Instance.InGameSettingUI.gameObject.SetActive(false);
        UIManager.Instance.WarningUI.gameObject.SetActive(true);
        UIManager.Instance.WarningUI.SetMessage();
    }

    public void OnMusicValueChange()
    {
        
    }
}
