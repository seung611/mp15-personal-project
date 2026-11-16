using UnityEngine;
using UnityEngine.UI;

public class TitleSetting : MonoBehaviour
{
    [SerializeField] private Button _escButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Slider _musicSlider;
    
    public void OnClickEsc()
    {
        UIManager.Instance.TitleSettingUI.gameObject.SetActive(false);
    }
    
    public void OnClickExitButton()
    {
        UIManager.Instance.WarningUI.gameObject.SetActive(true);
        UIManager.Instance.TitleSettingUI.gameObject.SetActive(false);
    }

    public void OnMusicValueChange()
    {
        
    }
}
