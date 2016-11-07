using UnityEngine;
using System.Collections;

public abstract class ability {

    public Sprite abilitySprite;
    public int range { get; protected set; }

    //other animation/asset data here

    public abstract void activate(Unit self, Unit other);

    public abstract ability clone();

    public virtual void setRange(int i)
    {
        this.range = i;
    }
}
