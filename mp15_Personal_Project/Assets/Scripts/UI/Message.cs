using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Message : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private float _showDuration = 0.5f;
    
    private CancellationTokenSource _hideToken;
    
    public void ShowMessage(string message)
    {
        gameObject.SetActive(true);
        _messageText.text = message;
        
        RestartHideTimer();
    }
    
    private void RestartHideTimer()
    {
        CancelHideTimer();
        
        _hideToken = new CancellationTokenSource();
        
        HideAfterDelay(_hideToken.Token).Forget();
    }

    private async UniTaskVoid HideAfterDelay(CancellationToken token)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_showDuration), 
            cancellationToken: token, ignoreTimeScale: true);
        
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void CancelHideTimer()
    {
        if (_hideToken != null)
        {
            try
            {
                if (!_hideToken.IsCancellationRequested)
                {
                    _hideToken.Cancel();
                }
            }
            catch (ObjectDisposedException) { /* 이미 폐기 중인 경우 무시 */ }
            
            _hideToken.Dispose();
            _hideToken = null; 
        }
    }
}
