using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Part {

    public enum slot { Undefined, weapon1, weapon2, core };

    public String displayName;
    public Guid partId;
    public Unit owner { get; private set; }
    public List<slot> slots { get; private set; }

    public attributes baseAtt { get; protected set; }
    public List<ability> abilities { get; private set; }

    public Part()
    {
        this.slots = new List<slot>();
        this.baseAtt = new attributes();
        this.abilities = new List<ability>();
    }

    public void setOwner(Unit newOwner)
    {
        this.owner = newOwner;
    }

    public void copyFrom(Part original)
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

    public virtual Part clone()
    {
        Part ret = new Part();
        Part.copy(this, ret);
        return ret;
    }

    protected static void copy(Part src, Part tgt)
    {
        tgt.displayName = src.displayName;

        tgt.partId = Guid.NewGuid();

        tgt.baseAtt.setTo(src.baseAtt);

        foreach (slot s in src.slots)
        {
            tgt.slots.Add((Part.slot)((int)s));
        }

        foreach (ability a in src.abilities)
        {
            ability newA = a.clone();
            newA.parent = tgt.owner;
            tgt.abilities.Add(newA);
        }
    }
}
