using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class unitBase : MonoBehaviour {

    //none of this class may be necessary, this lazy inheriting thing doesn't actually help since parts
    //and mechs don't have that much in common

    public String displayName;

    public attributes baseAtt { get; protected set; }
    //rest of the attributes in here
  
    public virtual void Initialize()
    {
        this.baseAtt = new attributes();
    }

    public abstract void GameInit();

    //cues
    public abstract void onTurnStart();

    public abstract void onTurnEnd();

    public abstract void onDeath();

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
}
