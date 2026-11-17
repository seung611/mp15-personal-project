using UnityEngine;

[CreateAssetMenu(fileName = "New Node", menuName = "Skill/Node")]
public class Node : ScriptableObject
{
    [SerializeField] private string _skillId;
    [SerializeField] private string _skillName;
    [TextArea] [SerializeField] private string _description;
    [SerializeField] private int _skillPrice;
    [SerializeField] private int _effectValue;
    [SerializeField] private Node _prevNode;
    
    public string SkillId => _skillId;
    public string SkillName => _skillName;
    public string Description => _description;
    public int SkillPrice => _skillPrice;
    public int EffectValue => _effectValue;
    public Node PrevNode => _prevNode;
}
