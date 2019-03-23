using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class GoapAction : MonoBehaviour, IComparer<GoapAction>
{
    public Stats stats;

    public Skill skillLeftStance { get; protected set; }
    public Skill skillRightStance { get; protected set; }
    public bool sequenced { get; private set; }
    public ActionAttributes actionAttributes;

    public HashSet<KeyValuePair<string, object>> Preconditions { get; protected set; }
    public HashSet<KeyValuePair<string, object>> Effects { get; protected set; } 

    protected void AddAction()
    {
        GoapAgent.allActions++;
    }

    public void SequenceThisAction(bool isSequenced)
    {
        sequenced = isSequenced;
    }

    public void ResetAction()
    {
        sequenced = false;
    }

    public virtual bool CheckPreconditions()
    {
        if (sequenced)
            return false;

        if (CPUControls.cpuLeftStance)
        {
            if (!SkillPreconditionsOK(skillLeftStance))
                return false;
        }
        else
        {
            if (!SkillPreconditionsOK(skillRightStance))
                return false;
        }

        return true;
    }

    protected bool SkillPreconditionsOK(Skill skill)
    {
        if (skill is Attack)
            if (stats.mana < skill.cost)
                return false;

        if (skill is Dodge || skill is Block)
            if (stats.stamina < skill.cost)
                return false;

        return true;
    }

    public void AddPrecondition(string key, object value)
    {
        Preconditions.Add(new KeyValuePair<string, object>(key, value));
    }

    public void RemovePrecondition(string key)
    {
        KeyValuePair<string, object> remove = default(KeyValuePair<string, object>);
        foreach (KeyValuePair<string, object> kvp in Preconditions)
        {
            if (kvp.Key.Equals(key))
                remove = kvp;
        }
        if (!default(KeyValuePair<string, object>).Equals(remove))
            Preconditions.Remove(remove);
    }

    public void AddEffect(string key, object value)
    {
        Effects.Add(new KeyValuePair<string, object>(key, value));
    }

    public void RemoveEffect(string key)
    {
        KeyValuePair<string, object> remove = default(KeyValuePair<string, object>);
        foreach (KeyValuePair<string, object> kvp in Effects)
        {
            if (kvp.Key.Equals(key))
                remove = kvp;
        }
        if (!default(KeyValuePair<string, object>).Equals(remove))
            Effects.Remove(remove);
    }
    public abstract string ActionName();

    public int Compare(GoapAction x, GoapAction y)
    {
        return x.actionAttributes.SuccessRate.CompareTo(y.actionAttributes.SuccessRate);
    }
}
