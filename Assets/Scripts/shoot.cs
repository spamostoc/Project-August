using UnityEngine;
using System.Collections;
using System;

public class shoot : ability
{
    public float damage;
    public int actionPointsCost;

    public shoot()
    {
        damage = 0;
        actionPointsCost = 0;
    }

    public shoot(float dmg, int apc)
    {
        damage = dmg;
        actionPointsCost = apc;
    }

    public override void activate(Unit self, Unit other)
    {
        self.onAttack(other, actionPointsCost);
        other.onDefend(self, damage);
    }

    public override ability clone()
    {
        shoot ret = new shoot();

        ret.abilitySprite = this.abilitySprite;
        ret.range = this.range;

        return ret;
    }
}
