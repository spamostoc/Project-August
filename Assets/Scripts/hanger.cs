using UnityEngine;
using System.Collections;

public class hanger : MonoBehaviour {

    public void makeTestMech()
    {
        mech newMech = new mech();

        attributes newAtt = new attributes();

        newAtt.health = 10;
        newAtt.movementPoints = 2;
        newAtt.actionPoints = 1;

        newMech.Initialize();

        newMech.att.setTo(newAtt);

        pManager.pDataManager.playerMechs.Add(newMech);
    }
}
