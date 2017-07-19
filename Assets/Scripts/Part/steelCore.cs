using UnityEngine;
using System.Collections;

public class SteelCore : Part
{
    public SteelCore() : base() { }

    public override Part clone()
    {
        SteelCore ret = new SteelCore();
        Part.copy(this, ret);
        return ret;
    }
}
