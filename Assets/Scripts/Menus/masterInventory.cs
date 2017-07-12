using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class masterInventory {

    private static IDictionary<part.slot, List<part>> allparts = new Dictionary<part.slot, List<part>>(); 

    public static part createPart(Type partType)
    {
        if (!partType.IsSubclassOf(typeof(part))) {
            throw new InvalidOperationException();
        }
        part newPart = UniTable.partDictionary[UniTable.classGuid[partType]].clone();
        foreach ( part.slot s in newPart.slots )
        {
            if( !allparts.ContainsKey(s) )
            {
                allparts.Add(s, new List<part>());
            }
            if (allparts[s].Contains(newPart))
            {
                throw new Exception("part somehow already exists");
            }
            allparts[s].Add(newPart);
        }
        return newPart;

    }

    public List<part> getParts()
    {
        List<part> ret = new List<part>();

        foreach (KeyValuePair<part.slot, List<part>> k in allparts)
        {
            ret.AddRange(k.Value);
        }

        return ret;
    }

    public List<part> getParts(part.slot slot)
    {
        return allparts[slot];
    }
}
