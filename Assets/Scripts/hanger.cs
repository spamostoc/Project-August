using UnityEngine;
using System.Collections;

public class hanger : MonoBehaviour {

    public void makeTestMech()
    {
        mech newMech = this.transform.gameObject.AddComponent<mech>();

        attributes newAtt = new attributes();

        newAtt.health = 10;
        newAtt.movementPoints = 3;
        newAtt.actionPoints = 1;

        newMech.att.setTo(newAtt);
        newMech.Initialize();

        newMech.MovementSpeed = 5;

        pManager.pDataManager.playerMechs.Add(newMech);
    }
}
