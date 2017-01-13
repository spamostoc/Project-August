﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public static class UniTable
{
    //this contain the instanced template classes to clone new objects from
    public static IDictionary<Type, Unit> unitDictionary = new Dictionary<Type, Unit>();
    public static IDictionary<Type, mechPart> partDictionary = new Dictionary<Type, mechPart>();
    public static IDictionary<Type, ability> abilityDictionary = new Dictionary<Type, ability>();
    public static IDictionary<Type, mechWeapon> weapondictionary = new Dictionary<Type, mechWeapon>();

    public static IDictionary<Type, Transform> prefabTable = new Dictionary<Type, Transform>();

    //this provide the guids that the save data is converted into
    public static readonly IDictionary<Type, Guid> classGuid = new Dictionary<Type, Guid>
    {
        { typeof(mech) , new Guid("5b00675d-621a-42d9-9ba9-1a570502c921")  },
        { typeof(mechPart), new Guid("08f6a56e-8f9a-4841-b4e3-49daff317ec5") },
        { typeof(ability) , new Guid("1f24132c-3821-4efa-86ef-953251122115") },
        { typeof(shoot) , new Guid("1addecfc-5cd5-4147-a7d9-a8034c2890d7") },
        { typeof(steelCore) , new Guid("ce81a106-f7e8-456d-b586-85d33753b3c5") }
    };

    public static Type getType(Guid id)
    {
        foreach (KeyValuePair<Type, Guid> classKey in UniTable.classGuid)
        {
            if (classKey.Value == id)
            {
                return classKey.Key;
            }
        }
        return null;
    }
}