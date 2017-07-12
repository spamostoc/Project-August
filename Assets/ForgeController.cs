using UnityEngine;
using System.Collections;

public class ForgeController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void spawnPart()
    {
        masterInventory.createPart(typeof(steelCore));
    }

    public void spawnWeapon()
    {
        masterInventory.createPart(typeof(lasGunWeapon));
    }

    public void spawnMech()
    {

    }
}
