using UnityEngine;

public class SkillTreeTest : MonoBehaviour
{
    private NodeTree _skillTree;

    private SkillNode _startNode;
    private SkillNode _trashCanNode;
    private SkillNode _compressorNode;
    private SkillNode _workNode;

    private void Start()
    {
        CreateTree();
        
        PrintTree(_startNode, 0);
        
        Debug.Log("=== First Node 해금 시도 ===");
        _skillTree.UnlockSkill(_startNode);
        PrintTree(_startNode, 0);

        Debug.Log("=== Work Skill 해금 시도 ===");
        bool result = _skillTree.UnlockSkill(_workNode);
        Debug.Log($"Work Skill 해금 결과: {result}");
        PrintTree(_startNode, 0);

        Debug.Log("=== TrashCan Skill 해금 시도 ===");
        bool trashCanResult = _skillTree.UnlockSkill(_trashCanNode);
        Debug.Log($"TrashCan Skill 해금 결과: {trashCanResult}");
        PrintTree(_startNode, 0);
        
    }

    private void CreateTree()
    {
        _startNode = new SkillNode(
            "StartNode",
            "FirstSkill",
            "Allows the use of a skill tree.",
            50);
        
        _trashCanNode = new SkillNode(
            "TrashCanNode",
            "TrashCanSkill",
            "Allows the use of a TrashCan skill.",
            50);
        
        _compressorNode = new SkillNode(
            "CompressorNode",
            "CompressorSkill",
            "Allows the use of a Compressor Skill.",
            50);
        
        _workNode = new SkillNode(
            "WorkNode",
            "WorkSkill",
            "Allows the use of a work skill.",
            50);
        
        _startNode.AddChild(_trashCanNode);
        _startNode.AddChild(_compressorNode);
        _trashCanNode.AddChild(_workNode);

        _skillTree = new NodeTree(_startNode, 100);
    }
    
    private void PrintTree(SkillNode node, int depth)
    {
        if (node == null) return;

        string indent = new string(' ', depth * 2);
        Debug.Log($"{indent}{node.SkillName} / {node._skillState} / Cost:{node.SkillPrice}");

        foreach (SkillNode child in node.children)
        {
            PrintTree(child, depth + 1);
        }
    }
}
