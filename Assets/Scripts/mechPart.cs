using UnityEngine;
using System.Collections;

public class mechPart : unitBase {

    public Unit parent;
    
    public void copyFrom(mechPart original)
    {
        this.att.setTo(original.att);
        this.buffs = original.copyBuffs();
        this.abilities = original.copyAbilities();
    }
}
