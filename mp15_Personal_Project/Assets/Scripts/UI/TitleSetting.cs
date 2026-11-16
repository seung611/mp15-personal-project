using UnityEngine;
using UnityEngine.UI;

public class TitleSetting : MonoBehaviour
{
    [SerializeField] private Button _escButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Slider _musicSlider;
    
    
    public void OnClickEsc()
    {
        UIManager.Instance._titleSettingUI.SetActive(false);
    }
    
    public void OnClickExitButton()
    {
        UIManager.Instance._titleSettingUI.SetActive(false);
        UIManager.Instance._warningUI.SetActive(true);
    }

    public void OnMusicValueChange()
    {
        
    }
}
