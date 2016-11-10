using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class classDictionary {

    public static readonly IDictionary<Type, Guid> unitGuids = new Dictionary<Type, Guid>
    {
        { typeof(mech) , new Guid("5b00675d-621a-42d9-9ba9-1a570502c921")  },
        { typeof(mechPart), new Guid("08f6a56e-8f9a-4841-b4e3-49daff317ec5") }
    };

    public static readonly IDictionary<Type, Guid> abilityGuids = new Dictionary<Type, Guid>
    {
        { typeof(ability) , new Guid("1f24132c-3821-4efa-86ef-953251122115") },
        { typeof(shoot) , new Guid("1addecfc-5cd5-4147-a7d9-a8034c2890d7") }
    };
}
