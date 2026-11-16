using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Warning : MonoBehaviour
{
    [SerializeField] private GameExit _gameExit;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _discription;
    [SerializeField] private Button _yes;
    [SerializeField] private Button _no;
    
    private string _titleWarningDescription = "";
    private string _inGameWarningDescription = "";

    public void OnClickTitleYes()
    {
        _gameExit.ExitGame();
    }

    public void OnClickInGameYes()
    {
        UIManager.Instance._warningUI.SetActive(false);
    }

    public void OnClickNo()
    {
        UIManager.Instance._warningUI.SetActive(false);
    }

    public void SetMessage()
    {
        _title.text = $"[Warning]";
        _discription.text = "Not saved. Are you sure you want to leave?";
    }
}
