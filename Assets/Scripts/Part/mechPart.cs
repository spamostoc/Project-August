using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class mechPart : unitBase {

    public Unit parent;
    public List<ability> abilities;
    
    public void copyFrom(mechPart original)
    {
        this.baseAtt = new attributes(original.baseAtt);
    }

    public override void GameInit()
    {
        throw new NotImplementedException();
    }

    public override void onTurnStart()
    {
        throw new NotImplementedException();
    }

    public override void onTurnEnd()
    {
        throw new NotImplementedException();
    }

    public override void onDeath()
    {
        throw new NotImplementedException();
    }
}
