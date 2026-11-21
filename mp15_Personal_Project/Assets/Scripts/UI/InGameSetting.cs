using UnityEngine;
using UnityEngine.UI;

public class InGameSetting : MonoBehaviour
{
    [SerializeField] private Button _escButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _titleButton;
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
    
    public void OnClickContinueButton()
    {
        UIManager.Instance.OnSettingUI();
    }
    
    public void OnClickTitleButton()
    {
        UIManager.Instance.InGameSettingUI.gameObject.SetActive(false);
        UIManager.Instance.WarningUI.gameObject.SetActive(true);
        // 작업 중인지, 결과 화면에서 열었는지에 따라 문구가 다름
        EWarningType type = GameManager.Instance.IsWorking ? EWarningType.LeaveWork : EWarningType.LeaveResult;
        UIManager.Instance.WarningUI.SetMessage(type);
    }

    public void OnMusicValueChange(float value)
    {
        SoundManager.Instance.SetMasterVolume(value);
    }
}
