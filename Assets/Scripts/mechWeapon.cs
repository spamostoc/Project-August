using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class mechWeapon
{

    public Unit parent;
    public List<ability> abilities;

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

    public abstract void Initialize();

    public abstract void onTurnStart();

    public abstract void onTurnEnd();
}
