using UnityEngine;
using System.Collections;

public class lasGunWeapon : mechWeapon {

    public override mechWeapon clone()
    {
        lasGunWeapon ret = new lasGunWeapon();
        ret.Initialize();
        mechWeapon.copy(this, ret);
        return ret;
    }
}
