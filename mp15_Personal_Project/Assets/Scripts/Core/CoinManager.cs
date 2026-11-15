using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private GarbageBagController _garbageBagController;

    private int _haveCoin;
    private int _roundCoin;
    
    private void OnEnable() => BindInputAction();

    private void OnDisable() => UnbindInputAction();
    
    private void BindInputAction()
    {
        GameManager.Instance.OnWorkEnded += ResultReward;
    }

    private void UnbindInputAction()
    {
        GameManager.Instance.OnWorkEnded -= ResultReward;
    }

    private void ResultReward()
    {
        _roundCoin = _garbageBagController.TiedTrashBag;
        _haveCoin += _roundCoin;
        
        Debug.Log($"획득 : {_roundCoin}, 보유 : {_haveCoin}");
    }
}
