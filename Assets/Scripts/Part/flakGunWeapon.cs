using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class FlakGunWeapon : MechWeapon
{
    public FlakGunWeapon () : base() { }

    public override Part clone()
    {
        FlakGunWeapon ret = new FlakGunWeapon();
        MechWeapon.copy(this, ret);
        return ret;
    }
}
