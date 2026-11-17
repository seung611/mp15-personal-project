using System;
using UnityEngine;
using UnityEngine.Rendering;

public class NodeTree
{
    private SkillNode _rootNode;
    private int _price;

    private void Start()
    {
        _rootNode = CreateStartSkillTree();

        PrintSkillTree(_rootNode, 0);
    }

    public NodeTree(SkillNode root, int price)
    {
        _rootNode = root;
        _price = price;
    }

    private SkillNode CreateStartSkillTree()
    {
        SkillNode start = new SkillNode(
            "StartNode",
            "FirstSkill",
            "Allows the use of a skill tree.",
            50);

        SkillNode trashCan = new SkillNode(
            "TrashCanNode",
            "TrashCanSkill",
            "Allows the use of a TrashCan skill.",
            50);

        SkillNode compressor = new SkillNode(
            "CompressorNode",
            "CompressorSkill",
            "Allows the use of a Compressor Skill.",
            50);

        SkillNode work = new SkillNode(
            "WorkNode",
            "WorkSkill",
            "Allows the use of a work skill.",
            50);
        
        start.AddChild(trashCan);
        start.AddChild(compressor);
        start.AddChild(work);

        start._skillState = SkillState.Available;
        
        return start;
    }

    private bool CanUnLock(SkillNode node)
    {
        if (Can(node)) return false;
        if (node.Parent == null) return true;
        
        return node.Parent._skillState == SkillState.Unlocked;
    }

    private bool Can(SkillNode node)
    {
        return (node == null
                || node._skillState == SkillState.Unlocked
                || _price < node.SkillPrice);
    }

    public bool UnlockSkill(SkillNode node)
    {
        if (!CanUnLock(node)) return false;
        
        _price -= node.SkillPrice;
        node._skillState = SkillState.Unlocked;

        UpdateChildrenState(node);
        
        return true;
    }

    private void UpdateChildrenState(SkillNode node)
    {
        foreach (SkillNode child in node.children)
        {
            if (child._skillState == SkillState.Locked)
            {
                child._skillState = SkillState.Available;
            }
        }
    }

    private void PrintSkillTree(SkillNode node, int depth)
    {
        if (node == null) return;
        
        string indent = new string(' ', depth * 2);
        Debug.Log($"{indent}{node.SkillName} / {node._skillState}");

        foreach (SkillNode child in node.children)
        {
            PrintSkillTree(child, depth + 1);
        }
    }
}
