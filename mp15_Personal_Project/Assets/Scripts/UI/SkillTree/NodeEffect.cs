using UnityEngine;

public abstract class NodeEffect : MonoBehaviour
{
    protected SkillNodeButton _node;
    public abstract EffectType EffectType { get;}
    public abstract void SetNodeEffect();
}
