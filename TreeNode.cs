using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode
{
    public DialogueNode dNode { get; private set; }
    public int parentIndex { get; private set; }

    public TreeNode(DialogueNode n, int i)
    {
        dNode = n;
        parentIndex = i;
    }
}
