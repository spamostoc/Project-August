using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class FlakGunWeapon : MechWeapon
{
    public override Part clone()
    {
        FlakGunWeapon ret = new FlakGunWeapon();
        ret.Initialize();
        MechWeapon.copy(this, ret);
        return ret;
    }
}
