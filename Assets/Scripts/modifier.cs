using UnityEngine;
using System.Collections;

public abstract class modifier : MonoBehaviour {

    public attributes att { get; private set; }


    public int duration { get; private set; }
    public int remainingDuration;

    public Unit parent;

    //other attributes here

    public void setAttributes(attributes newAttribute)
    {
        this.att.setTo(newAttribute);
    }

    public virtual void setDuration(int newDuration)
    {
        this.duration = newDuration;
    }

    public virtual void onTurnStart()
    {
            remainingDuration--;

        if (remainingDuration <= 0)
        {
            this.onEnd();
        }
    }

    public virtual void onTurnEnd() { }

    public virtual void onAttack(Unit other) { }

    public virtual void onDefend(Unit other) { }

    public virtual void onStart()
    {
        this.remainingDuration = this.duration;
    }

    public virtual void onEnd()
    {
        parent.buffs.Remove(this);
    }

    public abstract modifier clone();
}
