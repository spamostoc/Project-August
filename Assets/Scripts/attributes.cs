using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class attributes
{
    public float health;
    public float maxHealth;
    public int movementPoints;
    public int maxMovementPoints;
    public int mainActionPoints;
    public int maxMainActionPoints;
    public int bonusActionPoints;
    public int maxBonusActionPoints;
    public float armor;
    public float heatReduceRate;
    public float shieldPoints;
    public float maxShieldPoints;
    public float shieldRegenRate;
    public float shieldMitigation;

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

    public void add(attributes att)
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
