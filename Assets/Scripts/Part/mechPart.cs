using UnityEngine;
using System.Collections;

public class mechPart : unitBase {

    public Unit parent;
    
    public void copyFrom(mechPart original)
    {
        this.baseAtt = new attributes(original.baseAtt);
        this.buffs = original.copyBuffs();
        this.abilities = original.copyAbilities();
    }
}
