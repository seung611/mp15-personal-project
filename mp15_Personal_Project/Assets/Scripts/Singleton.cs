using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                FindInstance();
            }

            return _instance;
        }
    }

    protected void SetSingleton()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }

    private static void FindInstance()
    {
        _instance = FindFirstObjectByType<T>();

        if (_instance == null)
        {
            return;
        }
        
        DontDestroyOnLoad(_instance.gameObject);
    }
}
