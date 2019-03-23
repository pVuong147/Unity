using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoapPlanner {

    private List<GoapNode> leaves;
    private GoapNode startingNode;
    private HashSet<GoapAction> usableActions;
    private HashSet<GoapAction> subsetActions;
    private PlannerActions plannedActions;
    private PlannerActions counterActions;

    public GoapPlanner()
    {
        leaves = new List<GoapNode>();
        startingNode = new GoapNode();
        usableActions = new HashSet<GoapAction>();
        subsetActions = new HashSet<GoapAction>();
        plannedActions = new PlannerActions();
        counterActions = new PlannerActions();
    }

    public Queue<GoapAction> Plan(HashSet<GoapAction> availableActions,
                                  HashSet<KeyValuePair<string, object>> actualState,
                                  HashSet<KeyValuePair<string, object>> goal, 
                                  Queue<ActionFitAttribute> attributesToCompare)
    {
        // reset the actions to their initial state
        foreach (GoapAction a in availableActions)
            a.ResetAction();

        // check which actions can run
        usableActions.Clear();
        foreach (GoapAction a in availableActions)
            if (a.CheckPreconditions())
                usableActions.Add(a);

        // build up the tree from the actual state
        leaves.Clear();

        startingNode.SetGoapNode(null, null, actualState);
        bool success = BuildTree(startingNode, leaves, usableActions, goal);

        if (!success)
            return null;

        // find the cheapest leaf
        GoapNode optimal = null;
        optimal = FindBestLeaf(optimal, leaves, attributesToCompare);

        // work back through the parents from the best node
        plannedActions.ResetActions();
        if (optimal == null)
            return plannedActions.sortedActionsQueue;
        GoapNode n = optimal;
        while (n != null)
        {
            if (n.action != null)
                plannedActions.InsertAction(n.action);;
            n = n.GetParentNode();
        }
        plannedActions.ActionsToQueue();

        return plannedActions.sortedActionsQueue;        
    }
    private bool BuildTree(GoapNode currentNode, List<GoapNode> leaves, 
        HashSet<GoapAction> usableActions, HashSet<KeyValuePair<string, object>> goal)
    {
        bool foundSolution = false;

        foreach (GoapAction action in usableActions)
        {
            if (StateMatch(action.Preconditions, currentNode.state))
            {
                HashSet<KeyValuePair<string, object>> currentState = UpdateState(currentNode.state, action.Effects);
                ActionAttributes attributes = new ActionAttributes(currentNode.actionAttributes, action.actionAttributes);
                GoapNode node = new GoapNode(currentNode, action, attributes, currentState);
                
                if (StateMatch(goal, currentState))
                {
                    leaves.Add(node);
                    foundSolution = true;
                }
                else
                {
                    subsetActions = NewActionSet(usableActions, action);
                    bool found = BuildTree(node, leaves, subsetActions, goal);
                    if (found)
                        foundSolution = true;
                }
            }
        }

        return foundSolution;
    }
    public Queue<GoapAction> FindCounterAction(GoapAction action, bool cpuAttacking, CounterAction[] counters)
    {
        counterActions.ResetActions();

        if (cpuAttacking)
        {// find attacks that won't be blocked/dodged
            foreach (CounterAction counter in actionCounters)
            {
                if (counter.SuccessfulAttack(action))
                    if (!counter.attack.sequenced)
                        counterActions.AddActionToList(counter.attack);
            }
        }
        else
        {// find successful blocks/dodges
            foreach (CounterAction counter in actionCounters)
            {
                if (counter.SuccessfulDefense(action))
                    if (!counter.defense.sequenced)
                        counterActions.AddActionToList(counter.defense);
            }
        }

        // sort by the success rate of an action (descending)
        counterActions.SortActions(ActionFitAttribute.SuccessRate, true);
        counterActions.AddActionToQueue(0);

        return counterActions.sortedActionsQueue;
    }
}