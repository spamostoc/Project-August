using UnityEngine;
using System.Collections;

public class switchWeapon : ability {

    public int actionPointsCost;
    public int bonusActionPointsCost;

    public switchWeapon()
    {
        actionPointsCost = 0;
        bonusActionPointsCost = 0;
        targeted = false;
    }

    public override void activate(Unit self, Unit other)
    {
        //deduct stuff from primary weapon
        Debug.Log("Switching weapons");
        foreach(mechWeapon w in ((mech)self).weapons)
        {
            if (w != ((mech)self).activeWeapon)
            {
                Debug.Log("Switching to:" + w);
                ((mech)self).activeWeapon = w;
                break;
            }
        }
    }

    public override ability clone()
    {
        switchWeapon ret = new switchWeapon();
        ability.copy(this, ret);
        return ret;
    }
}
