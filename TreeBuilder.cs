using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBuilder
{
    private List<List<NodeInfo>> nodesOnEachLevel;
    private List<TreeNode> parentAllNodes;
    private List<TreeNode> childrenAllNodes;

    public TreeBuilder(List<List<NodeInfo>> nodes)
    {
        nodesOnEachLevel = nodes;
        parentAllNodes = new List<TreeNode>();
        childrenAllNodes = new List<TreeNode>();
    }
    public void CopyChildrenNodes(List<TreeNode> list)
    {
        childrenAllNodes = new List<TreeNode>();
        foreach (TreeNode item in list)
            childrenAllNodes.Add(item);
    }
    public DialogueNode CreateTree()
    {
        DialogueNode dialogue;
        // create a tree of DialogueNodes from the list of all nodes
        // nodesOnEachLevel is sorted in a way that the first node is the last node of the tree (bottom right node)
        List<DialogueNode> dialogueNodesOnLevel = new List<DialogueNode>();
        for (int i = 0; i < nodesOnEachLevel.Count; i++) // building a tree from the bottom
        {
            for (int j = 0; j < nodesOnEachLevel[i].Count; j++) // for each node on the current level
            {
                NodeInfo node = nodesOnEachLevel[i][j];
                DialogueNode dNode;
                // if it's the bottom level of a tree, all nodes are the last ones (they have no children)
                if (i == 0)
                    dNode = new DialogueNode(node.ActionNames);
                else
                {
                    List<DialogueNode> childrenOfNode = new List<DialogueNode>();
                    // find all children of dNode
                    foreach (TreeNode n in childrenAllNodes)
                        if (n.parentIndex == j)
                            childrenOfNode.Add(n.dNode);
                    if (childrenOfNode.Count == 0)
                        dNode = new DialogueNode(node.IsActionNode, node.ActionNames);
                    else
                        dNode = new DialogueNode(childrenOfNode);
                }
                dNode.SetSentences(node.Sentences);
                dNode.FillChoices(node.Choices);
                parentAllNodes.Add(new TreeNode(dNode, node.ParentNodeIndex));
            }
            // copy all "parent" nodes from i-level as a new list of potential children nodes for upper level nodes of the tree
            CopyChildrenNodes(parentAllNodes);
            parentAllNodes.Clear();
        }
        dialogue = childrenAllNodes[0].dNode;

        return dialogue;
    }
}