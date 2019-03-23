using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GoapAgent : MonoBehaviour {

    public bool ready { get; private set; }
    // planner plans the queue of actions required to achieve the current goal
    private GoapPlanner planner;
    // all available actions for the planner
    private HashSet<GoapAction> availableActions;
    // actions prepared for the current goal
    private Queue<GoapAction> currentActions;

    private List<HashSet<KeyValuePair<string, object>>> goals;
    private bool goalCompleted;
    // action attributes that are used for planning
    private Queue<ActionFitAttribute> fitAttributesToCompare;
    // CombatSequence manages the order of skills for both fighting bots
    public CombatSequence combatSequence;
    // actions that are effective counter moves to a specific player action
    public ActionCounters[] actionCounters;

    // Use this for initialization
    void Start () {

        availableActions = new HashSet<GoapAction>();
        currentActions = new Queue<GoapAction>();
        planner = new GoapPlanner();
        goalCompleted = false;
        fitAttributesToCompare = new Queue<ActionFitAttribute>();

    }
	public void AddActions()
    {
        for (int i = 0; i < transform.childCount - 1; i++)
            availableActions.Add(transform.GetChild(i).GetComponent<GoapAction>());
        ready = true;
    }
    public void SetGoal()
    {
        goals.Clear();
        if (cpuAttacking)
            goals.Add(goalAttack);
        else
            goals.Add(goalDefend);
    }
    public HashSet<KeyValuePair<string, object>> GetCurrentGoal()
    {
        return goals[0];
    }
    public bool GoalAchieved()
    {
        return goalCompleted;
    }
    public void ResetGoal()
    {
        goalCompleted = false;
    }
    public bool HavePlan(HashSet<KeyValuePair<string, object>> actualState, HashSet<KeyValuePair<string, object>> goal, Stats stats, LastAction lastAction)
    {
        if (Sequence.cpuFirst)// 1st pick - GOAP
        {
            SetFitAttributesPriority(goal, stats, lastAction);
            currentActions = planner.Plan(availableActions, actualState, goal, fitAttributesToCompare);
        }
        else// 2nd pick - find an action that counters player's action
        {
            Skill playerSk = combatSequence.sequence.playerSequence[Sequence.roundCount].skill;
            if (playerSk != null)
            {
                GoapAction action1st = availableActions.Where(w => w.skillName == playerSk.skillName).FirstOrDefault();
                currentActions = planner.FindCounterAction(action1st, Sequence.cpuAttacking, actionCounters);
            }
        }
        if (currentActions.Count == 0)
            currentActions.Enqueue(GetRandomNonSeqAction());
        currentActions.Peek().SequenceThisAction(true);
        if (CPUControls.cpuLeftStance)
            CombatSequence.cpuSkill = currentActions.Dequeue().skillLeftStance;
        else
            CombatSequence.cpuSkill = currentActions.Dequeue().skillRightStance;
        combatSequence.sequence.AddOpponentSkill(CombatSequence.cpuSkill);
        combatSequence.sequence.ShowOppSkill(true);
        return true;
    }
    public void ResetSequencedActions()
    {
        foreach (var item in availableActions)
        {
            if (item.sequenced)
                item.SequenceThisAction(false);
        }
    }
}
