using System.Collections.Generic;
using UnityEngine;

public class ConveyorManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _onConveyors;
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _direction;

    private void Update()
    {
        for (int i = 0; i <= _onConveyors.Count - 1; i++)
        {
            _onConveyors[i].GetComponent<Rigidbody>().velocity += _speed * _direction * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _onConveyors.Add(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        _onConveyors.Remove(collision.gameObject);
    }
}
