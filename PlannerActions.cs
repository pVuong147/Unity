using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlannerActions {

    public List<GoapAction> allActionsList { get; private set; }
    public Queue<GoapAction> sortedActionsQueue { get; private set; }

    public PlannerActions()
    {
        allActionsList = new List<GoapAction>();
        sortedActionsQueue = new Queue<GoapAction>();
    }

    public void ResetActions()
    {
        allActionsList.Clear();
        sortedActionsQueue.Clear();
    }
    public void InsertAction(GoapAction action)
    {
        allActionsList.Insert(0, action);
    }
    public void AddActionToList(GoapAction action)
    {
        allActionsList.Add(action);
    }
    public void AddActionToQueue(int actionIndex)
    {
        if (allActionsList.Count > 0)
            sortedActionsQueue.Enqueue(allActionsList[actionIndex]);
    }
    public void ActionsToQueue()
    {
        foreach (GoapAction action in allActionsList)
            sortedActionsQueue.Enqueue(action);
    }
    public void SortActions(ActionFitAttribute sortBy, bool descending)
    {
        switch (sortBy)
        {
            case ActionFitAttribute.Cost:
                if (descending)
                    allActionsList.Sort((x, y) => y.actionAttributes.Cost.CompareTo(x.actionAttributes.Cost));
                else
                    allActionsList.Sort((x, y) => x.actionAttributes.Cost.CompareTo(y.actionAttributes.Cost));
                break;
            case ActionFitAttribute.Damage:
                if (descending)
                    allActionsList.Sort((x, y) => y.actionAttributes.Damage.CompareTo(x.actionAttributes.Damage));
                else
                    allActionsList.Sort((x, y) => x.actionAttributes.Damage.CompareTo(y.actionAttributes.Damage));
                break;
            case ActionFitAttribute.SuccessRate:
                if (descending)
                    allActionsList.Sort((x, y) => y.actionAttributes.SuccessRate.CompareTo(x.actionAttributes.SuccessRate));
                else
                    allActionsList.Sort((x, y) => x.actionAttributes.SuccessRate.CompareTo(y.actionAttributes.SuccessRate));
                break;
            case ActionFitAttribute.ReactionTime:
                if (descending)
                    allActionsList.Sort((x, y) => y.actionAttributes.ReactionTime.CompareTo(x.actionAttributes.ReactionTime));
                else
                    allActionsList.Sort((x, y) => x.actionAttributes.ReactionTime.CompareTo(y.actionAttributes.ReactionTime));
                break;
            case ActionFitAttribute.DefenseChance:
                if (descending)
                    allActionsList.Sort((x, y) => y.actionAttributes.DefenseChance.CompareTo(x.actionAttributes.DefenseChance));
                else
                    allActionsList.Sort((x, y) => x.actionAttributes.DefenseChance.CompareTo(y.actionAttributes.DefenseChance));
                break;
            default:
                break;
        }
    }
}
