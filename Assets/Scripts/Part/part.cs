using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class part {

    public String displayName;

    public attributes baseAtt { get; protected set; }

    public enum slot { weapon1, weapon2, core };

    public List<slot> slots { get; private set; }

    public Unit owner { get; private set; }
    public List<ability> abilities { get; private set; }

    public virtual void Initialize()
    {
        this.slots = new List<slot>();
        this.baseAtt = new attributes();
        this.abilities = new List<ability>();
    }

    public void setOwner(Unit newOwner)
    {
        this.owner = newOwner;
    }

    public void copyFrom(part original)
    {
        this.baseAtt = new attributes(original.baseAtt);
    }

    public virtual void GameInit()
    {
        throw new NotImplementedException();
    }

    public virtual void onTurnStart()
    {
        throw new NotImplementedException();
    }

    public virtual void onTurnEnd()
    {
        throw new NotImplementedException();
    }

    public virtual void onDeath()
    {
        throw new NotImplementedException();
    }

    public virtual part clone()
    {
        part ret = new part();
        ret.Initialize();
        part.copy(this, ret);
        return ret;
    }

    protected static void copy(part src, part tgt)
    {
        Debug.Log("this is part.copy");
        tgt.displayName = src.displayName;

        tgt.baseAtt.setTo(src.baseAtt);

        foreach (slot s in src.slots)
        {
            tgt.slots.Add((part.slot)((int)s));
        }

        foreach (ability a in src.abilities)
        {
            ability newA = a.clone();
            newA.parent = tgt.owner;
            tgt.abilities.Add(newA);
        }
    }
}
