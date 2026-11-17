using System.Collections.Generic;
using UnityEngine;

public class SkillNode
{
    private string _skillId;
    private string _skillName;
    private string _description;
    private int _skillPrice;
    
    public SkillState _skillState;
    public SkillNode Parent;
    public List<SkillNode> children = new List<SkillNode>();

    public string SkillId => _skillId;
    public string SkillName => _skillName;
    public string Description => _description;
    public int SkillPrice => _skillPrice;

    public SkillNode(string skillId, string skillName, string description, int skillPrice)
    {
        _skillId = skillId;
        _skillName = skillName;
        _description = description;
        _skillPrice = skillPrice;
        _skillState = SkillState.Locked;
    }
    
    public void AddChild(SkillNode child)
    {
        child.Parent = this;
        children.Add(child);
    }
}

public enum SkillState
{
    Locked,
    Available,
    Unlocked
}
