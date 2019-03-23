using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoapNode
{
    public GoapNode parent { get; private set; }
    public GoapAction action { get; private set; }
    public ActionAttributes actionAttributes { get; private set; }
    public HashSet<KeyValuePair<string, object>> state { get; private set; }

    public GoapNode()
    {
        actionAttributes = new ActionAttributes();
    }
    public GoapNode(GoapNode parent, GoapAction action, ActionAttributes attributes, HashSet<KeyValuePair<string, object>> state)
    {
        this.parent = parent;
        this.action = action;
        actionAttributes = new ActionAttributes(attributes);
        this.state = state;
    }

    public void SetGoapNode(GoapNode parent, GoapAction action, HashSet<KeyValuePair<string, object>> state)
    {
        parent = parent;
        this.action = action;
        if (actionAttributes != null)
            actionAttributes.ResetAttributes();
        this.state = state;
    }
    public GoapNode GetParentNode()
    {
        return parent;
    }
    public GoapNode Compare(GoapNode toCompare, Queue<ActionFitAttribute> attributesToCompare)
    {
        if (attributesToCompare.Count == 0)
        {
            int random = MyRandom.GiveRandom(0, 2);
            if (random == 0)
                return this;
            else
                return toCompare;
        }

        ActionFitAttribute attribute = attributesToCompare.Dequeue();
        switch (attribute)
        {
            case ActionFitAttribute.Cost:
                if (actionAttributes.Cost > toCompare.actionAttributes.Cost)
                    return toCompare;
                else if (actionAttributes.Cost == toCompare.actionAttributes.Cost)
                    Compare(toCompare, attributesToCompare);
                break;
            case ActionFitAttribute.Damage:
                if (actionAttributes.Damage < toCompare.actionAttributes.Damage)
                    return toCompare;
                else if (actionAttributes.Damage == toCompare.actionAttributes.Damage)
                    Compare(toCompare, attributesToCompare);
                break;
            case ActionFitAttribute.SuccessRate:
                if (actionAttributes.SuccessRate < toCompare.actionAttributes.SuccessRate)
                    return toCompare;
                else if (actionAttributes.SuccessRate == toCompare.actionAttributes.SuccessRate)
                    Compare(toCompare, attributesToCompare);
                break;
            case ActionFitAttribute.ReactionTime:
                if (actionAttributes.ReactionTime > toCompare.actionAttributes.ReactionTime)
                    return toCompare;
                else if (actionAttributes.ReactionTime == toCompare.actionAttributes.ReactionTime)
                    Compare(toCompare, attributesToCompare);
                break;
            case ActionFitAttribute.DefenseChance:
                if (actionAttributes.DefenseChance < toCompare.actionAttributes.DefenseChance)
                    return toCompare;
                else if (actionAttributes.DefenseChance == toCompare.actionAttributes.DefenseChance)
                    Compare(toCompare, attributesToCompare);
                break;
            default:
                break;
        }

        return this;
    }
}
public enum ActionFitAttribute
{
    Cost,
    Damage,
    SuccessRate,
    ReactionTime,
    DefenseChance
}
