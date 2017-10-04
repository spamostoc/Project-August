using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;


public static class masterInventory {

    private static IDictionary<Part.slot, List<Part>> allParts = new Dictionary<Part.slot, List<Part>>();

    private static IDictionary<Guid, Mech> playerMechs = new Dictionary<Guid, Mech>();

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

    public static void clearParts()
    {
        allParts = new Dictionary<Part.slot, List<Part>>();
    }

    public static void addMech(Guid guid, Mech m)
    {
        playerMechs.Add(new KeyValuePair<Guid, Mech>(guid, m));
    }

    public static List<Mech> getMechs()
    {
        return playerMechs.Values.ToList<Mech>();
    }

    public static Mech getMech(Guid guid)
    {
        return playerMechs[guid];
    }
}
