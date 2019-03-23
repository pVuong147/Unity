using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionAttributes {

    [SerializeField]
    private float cost;
    [SerializeField]
    private float damage;
    [SerializeField]
    private int successRate;
    [SerializeField]
    private float reactionTime;
    [SerializeField]
    private float defenseChance;

    public float Cost
    {
        get
        {
            return cost;
        }
    }
    public float Damage
    {
        get
        {
            return damage;
        }
    }
    public int SuccessRate
    {
        get
        {
            return successRate;
        }
    }
    public float ReactionTime
    {
        get
        {
            return reactionTime;
        }
    }
    public float DefenseChance
    {
        get
        {
            return defenseChance;
        }
    }

    public ActionAttributes() { }
    public ActionAttributes(ActionAttributes attributes)
    {
        this.cost = attributes.cost;
        damage = attributes.damage;
        successRate = attributes.successRate;
        this.reactionTime = attributes.reactionTime;
        defenseChance = attributes.defenseChance;
    }
    public ActionAttributes(ActionAttributes attributes1, ActionAttributes attributes2)
    {
        cost = attributes1.cost + attributes2.cost;
        damage = attributes1.damage + attributes2.damage;
        successRate = attributes1.successRate + attributes2.successRate;
        reactionTime = attributes1.reactionTime + attributes2.reactionTime;
        defenseChance = attributes1.defenseChance + attributes2.defenseChance;
    }
    public void SetAttributes(ActionAttributes attributes)
    {
        cost = attributes.cost;
        damage = attributes.damage;
        successRate = attributes.successRate;
        reactionTime = attributes.reactionTime;
        defenseChance = attributes.defenseChance;
    }
    public void ResetAttributes()
    {
        cost = 0;
        damage = 0;
        successRate = 0;
        reactionTime = 0;
        defenseChance = 0;
    }
}
