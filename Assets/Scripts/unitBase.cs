﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class unitBase : MonoBehaviour {

    public attributes baseAtt { get; protected set; }
    //rest of the attributes in here

    //leave current values for the mech class

    public List<modifier> buffs;

    public List<ability> abilities;

    //list of abilities BIG HUGE

    public virtual void Initialize()
    {
        this.baseAtt = new attributes();
        this.buffs = new List<modifier>();
        this.abilities = new List<ability>();
    }
    
    public virtual void GameInit()
    {
    }

    public virtual attributes getSummedAttributes()
    {
        attributes ret = new attributes(baseAtt);
        foreach( modifier m in buffs )
        {
            ret.addTo(m.att);
        }
        return ret;
    }

    //cues
    public virtual void onTurnStart()
    {
        foreach(modifier m in buffs)
        {
            m.onTurnStart();
        }
    }

    public virtual void onTurnEnd() {
        foreach (modifier m in buffs)
        {
            m.onTurnEnd();
        }
    }

    public virtual void onAttack(Unit other, int actionPointsCost, int bonusActionPointsCost)
    {
        foreach (modifier m in buffs)
        {
            m.onAttack(other);
        }
    }

    public virtual void onDefend(Unit other, float damage)
    {
        foreach (modifier m in buffs)
        {
            m.onDefend(other);
        }
    }

    public virtual void onKill(Unit other) { }

    public virtual void onDeath() { }
    //for abilities
    public virtual void onActivate() { }

    //utilites
    /// <summary>
    /// Method indicates if it is possible to attack unit given as parameter, from cell given as second parameter.
    /// </summary>
    public virtual bool isUnitReachable(Unit other, int range, Cell sourceCell)
    {
        if (sourceCell.GetDistance(other.Cell) <= range)
            return true;

        return false;
    }

    public List<modifier> copyBuffs()
    {
        List<modifier> newBuff = new List<modifier>();
        Debug.Log(this.buffs);
        foreach (modifier m in this.buffs)
        {
            newBuff.Add(m.clone());
        }

        return newBuff;
    }


    public List<ability> copyAbilities()
    {
        List<ability> newAbilities = new List<ability>();

        foreach (ability a in this.abilities)
        {
            newAbilities.Add(a.clone());
        }

        return newAbilities;
    }
}
