using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class masterInventory {

    private static IDictionary<Part.slot, List<Part>> allParts = new Dictionary<Part.slot, List<Part>>(); 

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
            if( !allParts.ContainsKey(s) )
            {
                allParts.Add(s, new List<Part>());
            }
            if (allParts[s].Contains(newPart))
            {
                throw new Exception("Part somehow already exists");
            }
            allParts[s].Add(newPart);
        }
        return newPart;

    }

    public static List<Part> getParts()
    {
        List<Part> ret = new List<Part>();

        foreach (KeyValuePair<Part.slot, List<Part>> k in allParts)
        {
            ret.AddRange(k.Value);
        }

        return ret;
    }

    public static List<Part> getParts(Part.slot slot)
    {
        return allParts[slot];
    }

    public static Part getPart(Guid guid)
    {
        Part ret;
        foreach (Part.slot slot in allParts.Keys)
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
        return allParts[slot].Find(p => p.partId == guid);
    }

    public static void clearInventory()
    {
        allParts = new Dictionary<Part.slot, List<Part>>();
    }
}
