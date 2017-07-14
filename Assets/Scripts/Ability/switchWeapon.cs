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

        if(((mech)self).activeWeapon == ((mech)self).parts[Part.slot.weapon1])
        {
            ((mech)self).activeWeapon = (MechWeapon) ((mech)self).parts[Part.slot.weapon2];
        }
        else
        {
            ((mech)self).activeWeapon = (MechWeapon)((mech)self).parts[Part.slot.weapon1];
        }
    }

    public override ability clone()
    {
        switchWeapon ret = new switchWeapon();
        ability.copy(this, ret);
        return ret;
    }
}
