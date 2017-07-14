using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class masterInventory {

    private static IDictionary<Part.slot, List<Part>> allparts = new Dictionary<Part.slot, List<Part>>(); 

    public static Part createPart(Type partType)
    {
        if (!partType.IsSubclassOf(typeof(Part))) {
            throw new InvalidOperationException();
        }
        Part newPart = UniTable.partDictionary[UniTable.classGuid[partType]].clone();
        foreach ( Part.slot s in newPart.slots )
        {
            if( !allparts.ContainsKey(s) )
            {
                allparts.Add(s, new List<Part>());
            }
            if (allparts[s].Contains(newPart))
            {
                throw new Exception("Part somehow already exists");
            }
            allparts[s].Add(newPart);
        }
        return newPart;

    }

    public List<Part> getParts()
    {
        List<Part> ret = new List<Part>();

        foreach (KeyValuePair<Part.slot, List<Part>> k in allparts)
        {
            ret.AddRange(k.Value);
        }

        return ret;
    }

    public List<Part> getParts(Part.slot slot)
    {
        return allparts[slot];
    }
}
