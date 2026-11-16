using UnityEngine;
using UnityEngine.UI;

public class Help : MonoBehaviour
{
    [SerializeField] private Button _escButton;
    
    public void OnClickEsc()
    {
        UIManager.Instance._helpUI.SetActive(false);
    }
}
