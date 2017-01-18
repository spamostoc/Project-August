using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class mechWeapon
{

    public mech parent;
    public List<ability> abilities;
    public Sprite iconSprite;

    public float damage;
    public float shieldDamage;
    public int range;
    public float accuracy;
    public float maxHeat;
    public float currentHeat;

    public int maxAmmo;
    public int currentAmmo;

    public float armorPierce;
    public float shieldBypass;

    public virtual void Initialize()
    {
        this.abilities = new List<ability>();
    }

    public virtual void onAttack(mech target)
    {
        //damage logic
        Debug.Log("do attack logic here");

        target.onDefend(parent, this.damage);
    }

    public virtual void onTurnStart()
    {
        if (this.currentHeat > 0)
            this.currentHeat = Math.Max(0, this.currentHeat - this.parent.dynamicAttributes.heatReduceRate);
    }

    public virtual void onTurnEnd()
    {

    }

    public virtual mechWeapon clone()
    {
        mechWeapon ret = new mechWeapon();
        ret.Initialize();
        mechWeapon.copy(this, ret);
        return ret;
    }

    protected static void copy(mechWeapon src, mechWeapon tgt)
    {
        tgt.iconSprite = Sprite.Instantiate(src.iconSprite);

        foreach (ability a in src.abilities)
        {
            ability newA = a.clone();
            newA.parent = tgt.parent;
            tgt.abilities.Add(newA);
        }

        tgt.damage = src.damage;
        tgt.shieldDamage = src.shieldDamage;
        tgt.range = src.range;
        tgt.accuracy = src.accuracy;
        tgt.maxHeat = src.maxHeat;
        tgt.currentHeat = src.currentHeat;

        tgt.maxAmmo = src.maxAmmo;
        tgt.currentAmmo = src.currentAmmo;

        tgt.armorPierce = src.armorPierce;
        tgt.shieldBypass = src.shieldBypass;
    }
}
