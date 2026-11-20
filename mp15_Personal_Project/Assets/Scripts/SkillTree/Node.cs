using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private NodeData _nodeData;
    [SerializeField] private SkillNodeButton _button;
    
    public NodeData NodeData => _nodeData;
    public SkillNodeButton Button => _button;
}
