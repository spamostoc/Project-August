using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class attributes
{
    public float health { get; set; }
    public float maxHealth { get; set; }
    public int movementPoints { get; set; }
    public int maxMovementPoints { get; set; }
    public int mainActionPoints { get; set; }
    public int maxMainActionPoints { get; set; }
    public int bonusActionPoints { get; set; }
    public int maxBonusActionPoints { get; set; }
    public float armor { get; set; }
    public float heatReduceRate { get; set; }
    public float shieldPoints { get; set; }
    public float maxShieldPoints { get; set; }
    public float shieldRegenRate { get; set; }
    public float shieldMitigation { get; set; }

    public attributes() { }

    public attributes(attributes att)
    {
        this.setTo(att);
    }

    public void setTo(attributes att)
    {
        this.health = att.health;
        this.maxHealth = att.maxHealth;
        this.movementPoints = att.movementPoints;
        this.maxMovementPoints = att.maxMovementPoints;
        this.mainActionPoints = att.mainActionPoints;
        this.maxMainActionPoints = att.maxMainActionPoints;
        this.bonusActionPoints = att.bonusActionPoints;
        this.maxBonusActionPoints = att.maxBonusActionPoints;
        this.armor = att.armor;
        this.heatReduceRate = att.heatReduceRate;
        this.shieldPoints = att.shieldPoints;
        this.maxShieldPoints = att.maxShieldPoints;
        this.shieldRegenRate = att.shieldRegenRate;
        this.shieldMitigation = att.shieldMitigation;
    }

    public void addTo(attributes att)
    {
        this.health += att.health;
        this.maxHealth += att.maxHealth;
        this.movementPoints += att.movementPoints;
        this.maxMovementPoints += att.maxMovementPoints;
        this.mainActionPoints += att.mainActionPoints;
        this.maxMainActionPoints += att.maxMainActionPoints;
        this.bonusActionPoints += att.bonusActionPoints;
        this.maxBonusActionPoints += att.maxBonusActionPoints;
        this.armor += att.armor;
        this.heatReduceRate += att.heatReduceRate;
        this.shieldPoints += att.shieldPoints;
        this.maxShieldPoints += att.maxShieldPoints;
        this.shieldRegenRate += att.shieldRegenRate;
        this.shieldMitigation += att.shieldMitigation;
    }
}
