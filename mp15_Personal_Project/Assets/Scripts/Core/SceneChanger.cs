using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    private SceneReference _nextScene;
    private Action _onFadeCompleteEvent;
    
    public void FadeOut(SceneReference nextScene, Action onFadeComplete)
    {
        _nextScene = nextScene;
        _onFadeCompleteEvent = onFadeComplete;
        _animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(_nextScene.SceneName);
        _onFadeCompleteEvent?.Invoke();
        _onFadeCompleteEvent = null;
        _animator.SetTrigger("FadeIn");
    }
}
