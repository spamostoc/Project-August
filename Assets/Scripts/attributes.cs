using UnityEngine;
using System.Collections;

public class attributes
{

    public float health { get; set; }
    public int movementPoints { get; set; }
    public int actionPoints { get; set; }


    public attributes()
    {
        health = 0;
        movementPoints = 0;
        actionPoints = 0;
    }
    public void setTo(attributes att)
    {
        this.health = att.health;
        this.movementPoints = att.movementPoints;
        this.actionPoints = att.actionPoints;
    }

    public void addTo(attributes att)
    {
        this.health += att.health;
        this.movementPoints += att.movementPoints;
        this.actionPoints += att.actionPoints;
    }
}
