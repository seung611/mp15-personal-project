using UnityEngine;
using UnityEngine.UI;

public class TitleSetting : MonoBehaviour
{
    [SerializeField] private Button _escButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Slider _musicSlider;
    
    public void OnClickExitButton()
    {
        UIManager.Instance.TitleSettingUI.gameObject.SetActive(false);
        UIManager.Instance.WarningUI.gameObject.SetActive(true);
        UIManager.Instance.WarningUI.SetMessage();
    }

    public void OnMusicValueChange()
    {
        
    }
}
