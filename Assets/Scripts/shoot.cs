using UnityEngine;
using System.Collections;
using System;

public class shoot : ability
{
    public float damage;
    public int actionPointsCost;
    public int bonusActionPointsCost;

    public shoot()
    {
        damage = 0;
        actionPointsCost = 0;
        bonusActionPointsCost = 0;
    }

    public shoot(float dmg, int apc)
    {
        damage = dmg;
        actionPointsCost = apc;
    }

    public override void activate(Unit self, Unit other)
    {
        //deduct stuff from primary weapon
        self.onAttack(other, actionPointsCost, bonusActionPointsCost);
        //use primary weapons stats on opponent
        other.onDefend(self, damage);
    }

    public override ability clone()
    {
        shoot ret = new shoot();

        ret.abilitySprite = this.abilitySprite;
        ret.range = this.range;
        ret.damage = this.damage;
        ret.actionPointsCost = this.actionPointsCost;

        return ret;
    }
}
