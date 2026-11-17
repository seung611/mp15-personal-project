using UnityEngine;

public class ApplyEffect : MonoBehaviour
{
    [SerializeField] private TrashBagController _trashBagController;

    private void Start() => PlayerManager.Instance.SetApplyEffect(this);

    public void AddTrashBagCapacity(float value)
    {
        _trashBagController.TrashBagCapacity += (int)value;
    }
    
}
