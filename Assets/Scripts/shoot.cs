using UnityEngine;
using System.Collections;
using System;

public class shoot : ability
{
    public override void activate(Unit self, Unit other)
    {
        throw new NotImplementedException();
    }

    public override ability clone()
    {
        shoot ret = new shoot();

        ret.abilitySprite = this.abilitySprite;

        return ret;
    }
}
