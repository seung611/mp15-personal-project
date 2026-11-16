using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private int _haveCoin;

    public int HaveCoin => _haveCoin;

    private void Awake() => SetSingleton();

    public void SaveCoin(int roundCoin)
    {
        _haveCoin += roundCoin;
        UIManager.Instance.InGameUI.SetCoinUI(_haveCoin, roundCoin);
    }

    public void UseCoin(int value)
    {
        _haveCoin -= value;
    }
}
