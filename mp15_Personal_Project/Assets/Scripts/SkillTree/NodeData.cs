using UnityEngine;

[CreateAssetMenu(fileName = "New Node", menuName = "Skill/Node")]
public class NodeData : ScriptableObject
{
    [SerializeField] private string _skillId;
    [SerializeField] private string _skillName;
    [TextArea] [SerializeField] private string _description;
    [SerializeField] private int _skillPrice;
    [SerializeField] private EffectType _effectType;
    [SerializeField] private float _effectValue;
    [SerializeField] private NodeData _prevNode;
    
    public string SkillId => _skillId;
    public string SkillName => _skillName;
    public string Description => _description;
    public int SkillPrice => _skillPrice;
    public EffectType EffectType => _effectType;
    public float EffectValue => _effectValue;
    public NodeData PrevNode => _prevNode;
}
