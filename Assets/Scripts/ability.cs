using UnityEngine;
using System.Collections;

public abstract class ability {

    public Sprite abilitySprite;

    //other animation/asset data here

    public abstract void activate(Unit self, Unit other);

    public abstract ability clone();
}
