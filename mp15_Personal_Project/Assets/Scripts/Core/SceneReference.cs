using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class SceneReference : ISerializationCallbackReceiver
{
    #if UNITY_EDITOR
    [SerializeField] private SceneAsset _sceneAsset;
    #endif

    [SerializeField] private string _sceneName = "";
    public string SceneName => _sceneName;

    public void OnBeforeSerialize()
    {
    #if UNITY_EDITOR
        if (_sceneAsset != null)
        {
            _sceneName = _sceneAsset.name;
        }
        else
        {
            _sceneName = "";
        }
    #endif
    }

    public void OnAfterDeserialize() { }
}