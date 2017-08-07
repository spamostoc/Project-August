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

    public override void activate(Unit other)
    {
        //deduct stuff from primary weapon
        Debug.Log("Switching weapons");

        Mech selfMech = (Mech)this.parent;
        if (null != selfMech.parts[Part.slot.weapon1] && selfMech.activeWeapon == selfMech.parts[Part.slot.weapon2])
        {
            selfMech.activeWeapon = (MechWeapon)selfMech.parts[Part.slot.weapon1];
        }
        else if (null != selfMech.parts[Part.slot.weapon2] && selfMech.activeWeapon == selfMech.parts[Part.slot.weapon1])
        {
            selfMech.activeWeapon = (MechWeapon)selfMech.parts[Part.slot.weapon2];
        }

    }

    public override ability clone()
    {
        switchWeapon ret = new switchWeapon();
        ability.copy(this, ret);
        return ret;
    }
}
