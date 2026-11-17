using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private NodeData _nodeData;
    [SerializeField] private SkillNodeButton _button;
    [SerializeField] private NodeEffect _nodeEffect;
    
    public NodeData NodeData => _nodeData;
    public NodeEffect NodeEffect => _nodeEffect;
    public SkillNodeButton Button => _button;
}
