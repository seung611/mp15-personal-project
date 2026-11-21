using UnityEngine;
using UnityEngine.UI;

public class TitleSetting : MonoBehaviour
{
    [SerializeField] private Button _escButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Slider _musicSlider;

    private void Awake()
    {
        _musicSlider.onValueChanged.AddListener(OnMusicValueChange);
    }

    private void OnEnable()
    {
        // 팝업을 열 때 현재 볼륨을 슬라이더에 표시 (이벤트는 발생시키지 않음)
        _musicSlider.SetValueWithoutNotify(SoundManager.Instance.MasterVolume);
    }

    private void OnDestroy()
    {
        _musicSlider.onValueChanged.RemoveListener(OnMusicValueChange);
    }
    
    public void OnClickExitButton()
    {
        UIManager.Instance.TitleSettingUI.gameObject.SetActive(false);
        UIManager.Instance.WarningUI.gameObject.SetActive(true);
        UIManager.Instance.WarningUI.SetMessage();
    }

    public void OnMusicValueChange(float value)
    {
        SoundManager.Instance.SetMasterVolume(value);
    }
}
