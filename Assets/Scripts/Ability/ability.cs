using UnityEngine;
using System.Collections;
using System;

public class ability {

    public Unit parent;
    public Sprite iconSprite;
    public int uiPriority;
    public Boolean targeted;
    protected int range { get; set; }

    //other animation/asset data here

    public virtual void Initialize()
    {

    }

    public virtual void activate(Unit other)
    {
        throw new NotImplementedException();
    }

    public virtual ability clone()
    {
        ability ret = new ability();
        ret.Initialize();
        ability.copy(this, ret);
        return ret;
    }

    public virtual void setRange(int i)
    {
        this.range = i;
    }

    public virtual int getRange()
    {
        return this.range;
    }

    protected static void copy(ability src, ability tgt)
    {
        tgt.parent = src.parent;
        tgt.iconSprite = Sprite.Instantiate(src.iconSprite);
        tgt.range = src.range;
        tgt.targeted = src.targeted;
    }
}
