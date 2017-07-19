using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class masterInventory {

    private static IDictionary<Part.slot, List<Part>> allparts = new Dictionary<Part.slot, List<Part>>(); 

    public static Part createPart(Type partType)
    {
        Debug.Log("creating new part of guid: " + partType);
        if (!partType.IsSubclassOf(typeof(Part))) {
            throw new InvalidOperationException("received part type: " + partType);
        }
        Part newPart = UniTable.partDictionary[UniTable.classGuid[partType]].clone();
        if (newPart.slots.Count == 0)
        {
            throw new IndexOutOfRangeException("New part has no defined slots: " + newPart);
        }
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

    public static List<Part> getParts()
    {
        List<Part> ret = new List<Part>();

        foreach (KeyValuePair<Part.slot, List<Part>> k in allparts)
        {
            ret.AddRange(k.Value);
        }

        return ret;
    }

    public static List<Part> getParts(Part.slot slot)
    {
        return allparts[slot];
    }

    public static Part getPart(Guid guid)
    {
        Part ret;
        foreach (Part.slot slot in allparts.Keys)
        {
            ret = getPart(guid, slot);
            if (null != ret)
            {
                return ret;
            }
        }
        return null;
    }

    public static Part getPart(Guid guid, Part.slot slot)
    {
        return allparts[slot].Find(p => p.partId == guid);
    }

    public static void clearInventory()
    {
        allparts = new Dictionary<Part.slot, List<Part>>();
    }
}
