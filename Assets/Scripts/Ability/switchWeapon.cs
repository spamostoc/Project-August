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

        if(((Mech)self).activeWeapon == ((Mech)self).parts[Part.slot.weapon1])
        {
            ((Mech)self).activeWeapon = (MechWeapon) ((Mech)self).parts[Part.slot.weapon2];
        }
        else
        {
            ((Mech)self).activeWeapon = (MechWeapon)((Mech)self).parts[Part.slot.weapon1];
        }
    }

    public override ability clone()
    {
        switchWeapon ret = new switchWeapon();
        ability.copy(this, ret);
        return ret;
    }
}
