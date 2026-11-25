using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] private Vector3 _beltDirection;
    [SerializeField] private ConveyorManager _conveyorManager;
    [SerializeField] private float _conveyorSpeed = 0.725f;
    
    private Material _material;
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        CacheComponents();
        Init();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody target = collision.rigidbody;

        if (target == null) return;

        _conveyorManager.AddCan(target);
    }
    private void OnCollisionExit(Collision collision)
    {
        Rigidbody target = collision.rigidbody;

        if (target == null) return;

        _conveyorManager.RemoveCan(target);
    }
    private void Update()
    {
        ConveyorBeltMover();
    }
    
    private void ConveyorBeltMover()
    {
        Vector3 direction = _beltDirection;
        
        Vector2 textureDirection = new Vector2(
            direction.x,
            direction.z
        ).normalized;
        
        _material.mainTextureOffset += 
            textureDirection * (_conveyorSpeed * Time.deltaTime);
    }
    
    private void Init()
    {
        if (_meshRenderer == null) return;
        
        _material = _meshRenderer.material;
    }

    private void CacheComponents()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }
}
