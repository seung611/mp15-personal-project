using UnityEngine;

public class NodeEffect : MonoBehaviour
{
    protected Node _node;
    public virtual EffectType EffectType { get;}

    private void Awake() => CacheComponents();

    public virtual void SetNodeEffect()
    {
    }

    private void CacheComponents()
    {
        _node = GetComponent<Node>();
    }
}
