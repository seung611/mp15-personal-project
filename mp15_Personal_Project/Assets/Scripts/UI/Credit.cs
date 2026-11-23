using UnityEngine;
using UnityEngine.UI;

public class Credit : MonoBehaviour
{
    [SerializeField] private Button _escButton;
    [SerializeField] private ScrollRect _scrollRect;
    
    public void OnClickEsc()
    {
        UIManager.Instance.CreditUI.gameObject.SetActive(false);
    }
}
