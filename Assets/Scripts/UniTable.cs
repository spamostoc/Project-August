using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public static class UniTable
{
    //this contain the instanced template classes to clone new objects from
    //use guids so that 1 class can serve as multiple templates
    public static IDictionary<Guid, Unit> unitDictionary = new Dictionary<Guid, Unit>();
    public static IDictionary<Guid, mechPart> partDictionary = new Dictionary<Guid, mechPart>();
    public static IDictionary<Guid, ability> abilityDictionary = new Dictionary<Guid, ability>();
    public static IDictionary<Guid, mechWeapon> weapondictionary = new Dictionary<Guid, mechWeapon>();

    public static IDictionary<Type, Transform> prefabTable = new Dictionary<Type, Transform>();

    //this provide the guids that the save data is converted into
    public static readonly IDictionary<Type, Guid> classGuid = new Dictionary<Type, Guid>
    {
        { typeof(mech) , new Guid("5b00675d-621a-42d9-9ba9-1a570502c921")  },
        { typeof(mechWeapon) , new Guid("f52dadd7-4273-49c4-81b0-28972842c745") },
        { typeof(flakGunWeapon) , new Guid("a089fa1b-8991-4df5-8049-5583abdcb740") },
        { typeof(ability) , new Guid("1f24132c-3821-4efa-86ef-953251122115") },
        { typeof(shoot) , new Guid("1addecfc-5cd5-4147-a7d9-a8034c2890d7") },
        { typeof(mechPart), new Guid("08f6a56e-8f9a-4841-b4e3-49daff317ec5") },
        { typeof(steelCore) , new Guid("ce81a106-f7e8-456d-b586-85d33753b3c5") }
    };

    public static Type GetTypeFromGuid(Guid id)
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