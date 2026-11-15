using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private GarbageTruckController _garbageTruckController;
    [SerializeField] private GarbageBagController _garbageBagController;

    private int _haveCoin;
    private int _roundCoin;
    
    private void OnEnable() => BindInputAction();

    private void OnDisable() => UnbindInputAction();
    
    private void BindInputAction()
    {
        _garbageTruckController.OnWorkEnded += ResultReward;
    }

    private void UnbindInputAction()
    {
        _garbageTruckController.OnWorkEnded -= ResultReward;
    }

    private void ResultReward()
    {
        _roundCoin = _garbageBagController.TiedTrashBag;
        _haveCoin += _roundCoin;
        
        Debug.Log($"획득 : {_roundCoin}, 보유 : {_haveCoin}");
    }
}
