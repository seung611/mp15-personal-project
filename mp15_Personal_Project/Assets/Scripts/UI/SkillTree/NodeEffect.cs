using UnityEngine;

public class NodeEffect : MonoBehaviour
{
    protected SkillNodeButton _node;
    public virtual EffectType EffectType { get;}

    private void Awake() => CacheComponents();

    public virtual void SetNodeEffect(ApplyEffect applyEffect)
    {
    }
    
    private void CacheComponents()
    {
        _node = GetComponent<SkillNodeButton>();
    }
}
