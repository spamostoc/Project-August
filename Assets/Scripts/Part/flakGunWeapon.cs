﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class flakGunWeapon : mechWeapon
{
    public override part clone()
    {
        flakGunWeapon ret = new flakGunWeapon();
        ret.Initialize();
        mechWeapon.copy(this, ret);
        return ret;
    }
}
