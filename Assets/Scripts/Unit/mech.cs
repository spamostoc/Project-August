using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class Mech : Unit {

    public MechWeapon activeWeapon;

    Coroutine PulseCoroutine;

    public override void Initialize()
    {
        base.Initialize();
        this.parts.Add(Part.slot.weapon1, null);
        this.parts.Add(Part.slot.weapon2, null);
        this.parts.Add(Part.slot.core, null);
    }
    
    public override void GameInit()
    {
        base.GameInit();

        foreach(KeyValuePair<Part.slot, Part> p in parts)
        {
            try
            {
                p.Value.GameInit();
            }
            catch (Exception e)
            {
                Debug.Log("this is a Part object");
            }
        }

        this.activeWeapon = (MechWeapon) this.parts[Part.slot.weapon1];
        //transform.position += new Vector3(0, 0, -1);
    }

    public override bool addPartAs(Part part, Part.slot slot)
    {
        return base.addPartAs(part, slot);
    }

    public override bool removePart(Part part)
    {
        throw new NotImplementedException();
    }

    public override void onAttack(Unit other, int mainActionPointsCost, int bonusActionPointsCost)
    {
        if (isMoving)
            return;
        if (!parseActionCost(mainActionPointsCost, bonusActionPointsCost))
            return;
        base.onAttack(other, mainActionPointsCost, bonusActionPointsCost);

        MarkAsAttacking(other);
        this.activeWeapon.onAttack((Mech)other);
        //do some attacking stuff here
    }

    public override bool IsCellMovableTo(Cell cell)
    {
        return base.IsCellMovableTo(cell) && (cell as MyOtherHexagon).GroundType != GroundType.Water;
        //Prohibits moving to cells that are marked as water.
    }

    public override bool IsCellTraversable(Cell cell)
    {
        return base.IsCellTraversable(cell) && (cell as MyOtherHexagon).GroundType != GroundType.Water;
        //Prohibits moving through cells that are marked as water.
    }

    public override void OnUnitDeselected()
    {
        base.OnUnitDeselected();
        StopCoroutine(PulseCoroutine);
        transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
    }

    public override void MarkAsAttacking(Unit other)
    {
        StartCoroutine(Jerk(other, 0.25f));
    }
    public override void MarkAsDefending(Unit other)
    {
        StartCoroutine(Glow(new Color(1, 0.5f, 0.5f), 1));
    }
    public override void MarkAsDestroyed()
    {
    }

    public override void onTurnStart()
    {
        base.onTurnStart();
    }

    public override void MarkAsFriendly()
    {
        SetHighlighterColor(new Color(0.75f, 0.75f, 0.75f, 0.5f));
    }
    public override void MarkAsReachableEnemy()
    {
        SetHighlighterColor(new Color(1, 0, 0, 0.5f));
    }
    public override void MarkAsSelected()
    {
        PulseCoroutine = StartCoroutine(Pulse(1.0f, 0.5f, 1.25f));
        SetHighlighterColor(new Color(0, 1, 0, 0.5f));
    }
    public override void MarkAsFinished()
    {
        SetColor(Color.gray);
        SetHighlighterColor(new Color(0.75f, 0.75f, 0.75f, 0.5f));
    }
    public override void UnMark()
    {
        SetHighlighterColor(Color.clear);
        SetColor(Color.white);
    }

    private void SetColor(Color color)
    {
        var _renderer = GetComponent<SpriteRenderer>();
        if (_renderer != null)
        {
            _renderer.color = color;
        }
    }
    private void SetHighlighterColor(Color color)
    {
        var highlighter = transform.Find("WhiteTile").GetComponent<SpriteRenderer>();
        if (highlighter != null)
        {
            highlighter.color = color;
        }
    }

    public void copyFrom(Mech original)
    {
        this.baseAtt.setTo(original.baseAtt);

        this.copyAbilitiesFrom(original);

        this.copyBuffsFrom(original);

        this.copyWeaponsFrom(original);

        this.MovementSpeed = original.MovementSpeed;
    }

    public void copyWeaponsFrom(Mech m)
    {
        this.parts[Part.slot.weapon1] = m.parts[Part.slot.weapon1].clone();
        this.parts[Part.slot.weapon2] = m.parts[Part.slot.weapon2].clone();
    }
}
