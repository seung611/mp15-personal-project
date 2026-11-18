using System;
using System.Collections.Generic;
using UnityEngine;

public class SuccessOrFail : MonoBehaviour
{
    [SerializeField] private Vector3 _beltDirection;
    [SerializeField] private float _conveyorSpeed = 0.5f;
    [SerializeField] private List<Can> _cans;
    [SerializeField] private ConveyorManager _conveyorManager;

    private Material _material;
    private MeshRenderer _meshRenderer;
    
    private void Awake()
    {
        CacheComponents();
        Init();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Can target = collision.transform.GetComponent<Can>();

        if (target == null) return;

        _conveyorManager.AddCan(target.GetComponent<Rigidbody>());
        _cans.Add(target);
        
        for (int i = 0; i < _cans.Count; i++)
        {
            if (target.IsSuccess)
            {
                _conveyorManager.LocalDirection = new Vector3(0f ,0f ,-1f);
                _beltDirection = new Vector3(0f, 0f, 1f);
            }
            else
            {
                _conveyorManager.LocalDirection = new Vector3(0f ,0f ,1f);
                _beltDirection = new Vector3(0f, 0f, -1f);
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        Can target = collision.transform.GetComponent<Can>();

        if (target == null) return;

        _conveyorManager.RemoveCan(target.GetComponent<Rigidbody>());
        _cans.Remove(target);
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
