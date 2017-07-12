using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class mechWeapon : part
{

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

    public String name;

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void GameInit()
    {
        this.currentAmmo = this.maxAmmo;
        this.currentHeat = 0;
    }

    public virtual void onAttack(mech target)
    {
        //damage logic
        Debug.Log("do attack logic here");
        if(this.currentAmmo <= 0)
        {
            return;
        }
        this.currentAmmo--;

        target.onDefend(owner, this.damage);
    }

    public override void onTurnStart()
    {
        if (this.currentHeat > 0)
            this.currentHeat = Math.Max(0, this.currentHeat - this.owner.dynamicAttributes.heatReduceRate);
    }

    public override void onTurnEnd()
    {

    }

    public override part clone()
    {
        mechWeapon ret = new mechWeapon();
        ret.Initialize();
        mechWeapon.copy(this, ret);
        return ret;
    }

    protected static void copy(mechWeapon src, mechWeapon tgt)
    {
        Debug.Log("this is mechWeapon.copy");
        part.copy(src, tgt);
        tgt.name = src.name;
        tgt.iconSprite = Sprite.Instantiate(src.iconSprite);

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
