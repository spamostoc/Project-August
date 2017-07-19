using UnityEngine;
using System.Collections;

public class LasGunWeapon : MechWeapon
{
    public LasGunWeapon() : base() { }

    public override Part clone()
    {
        LasGunWeapon ret = new LasGunWeapon();
        MechWeapon.copy(this, ret);
        return ret;
    }
}
