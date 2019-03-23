using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeInfo
{
    // These fields are filled in the inspector in the scriptable dialogue object
    [SerializeField]
    private int nodeLevel;
    [SerializeField]
    private int parentNodeIndex;
    [SerializeField]
    private List<string> sentences;
    [SerializeField]
    private List<string> choices;
    [SerializeField]
    private bool isActionNode;
    [SerializeField]
    private string[] actionNames;

    public int NodeLevel
    {
        get
        {
            return nodeLevel;
        }        
    }
    public int ParentNodeIndex
    {
        get
        {
            return parentNodeIndex;
        }        
    }
    public List<string> Sentences
    {
        get
        {
            return sentences;
        }        
    }
    public List<string> Choices
    {
        get
        {
            return choices;
        }        
    }
    public bool IsActionNode
    {
        get
        {
            return isActionNode;
        }        
    }
    public string[] ActionNames
    {
        get
        {
            return actionNames;
        }        
    }   
}
