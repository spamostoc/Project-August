using UnityEngine;
using System.Collections;
using System;

public class shoot : ability
{
    public int actionPointsCost;
    public int bonusActionPointsCost;

    public shoot()
    {
        actionPointsCost = 1;
        bonusActionPointsCost = 0;
        targeted = true;
    }

    public override void activate(Unit other)
    {
        //deduct stuff from primary weapon
        this.parent.onAttack(other, actionPointsCost, bonusActionPointsCost);
    }

    public override ability clone()
    {
        shoot ret = new shoot();
        ability.copy(this, ret);
        ret.actionPointsCost = this.actionPointsCost;
        ret.bonusActionPointsCost = this.bonusActionPointsCost;

        return ret;
    }

    public override int getRange()
    {
        return ((Mech)parent).activeWeapon.range;
    }
}
