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

    public override void activate(Unit self, Unit other)
    {
        //deduct stuff from primary weapon
        self.onAttack(other, actionPointsCost, bonusActionPointsCost);
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
        return ((mech)parent).activeWeapon.range;
    }
}
