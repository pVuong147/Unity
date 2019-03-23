using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentAI : MonoBehaviour {

    Stats stats;

    private State state;
    public static LastAction lastAction;
    public static bool inAction;

    public static HashSet<KeyValuePair<string, object>> actualState;
    public GoapAgent goapAgent;
	private bool agentHavePlan;

    // Use this for initialization
    void Start () {

        stats = GameObject.FindGameObjectWithTag("CPU").GetComponent<Stats>();
        inAction = false;
		agentHavePlan = false;
    }
	public void SetDifficulty(BotDifficulty diff)
    {
        StopAllCoroutines();
        switch (diff)
        {
            case BotDifficulty.Dummy:
                StartCoroutine(FSM_SwitchStance());
                break;
            case BotDifficulty.Acrobat:
                StartCoroutine(FSM_BlockDodge());
                break;
            case BotDifficulty.Boxer:
                StartCoroutine(FSM_Punches());
                break;
            case BotDifficulty.KungFu:
            	SetActualState();
                goapAgent.AddActions();
                StartCoroutine(FSM_GOAP());
                break;
            default:
                break;
        }
    }
    private IEnumerator FSM_GOAP()
    {
        while (true)
        {
            if (goapAgent.ready)
            {
                switch (state)
                {
                    case State.Idle:
                        if (!inAction)
                            Idle();
                        break;
                    case State.SetGoals:
                        goapAgent.SetGoal();
                        state = State.Plan;
                        break;
                    case State.Plan:
						agentHavePlan = false;
                        if (goapAgent.HavePlan(actualState, goapAgent.Goals[0], stats, lastAction))
                        {
							agentHavePlan = true;
                            state = State.Suspend;
                        }
                        break;
                    case State.Suspend:
                        Suspend();
                        break;
                    case State.Dead:
                        Dead();
                        break;
                    default:
                        break;
                }
            }
            yield return null;
        }
    }
}
