using UnityEngine;

public class CompressorSpace : MonoBehaviour
{
    [SerializeField] private Collider _plateCollider;   // 압축판(top) 콜라이더

    public Collider PlateCollider => _plateCollider;
}
