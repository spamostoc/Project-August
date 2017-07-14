using UnityEngine;
using System.Collections;

public class LasGunWeapon : MechWeapon {

    public override Part clone()
    {
        LasGunWeapon ret = new LasGunWeapon();
        ret.Initialize();
        MechWeapon.copy(this, ret);
        return ret;
    }
}
