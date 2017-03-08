using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class mech : Unit {

    public List<mechPart> parts;

    public List<mechWeapon> weapons;

    public mechWeapon activeWeapon;

    Coroutine PulseCoroutine;

    public override void Initialize()
    {
        base.Initialize();
        this.parts = new List<mechPart>();
        this.weapons = new List<mechWeapon>();
    }

    public override void GameInit()
    {
        base.GameInit();
        //hack to deal with aliens.cs
        if (this.weapons.Count == 0)
            return;

        foreach(mechWeapon w in weapons)
        {
            w.GameInit();
        }

        this.activeWeapon = this.weapons[0];
        //transform.position += new Vector3(0, 0, -1);
    }

    public override void onAttack(Unit other, int mainActionPointsCost, int bonusActionPointsCost)
    {
        if (isMoving)
            return;
        if (!parseActionCost(mainActionPointsCost, bonusActionPointsCost))
            return;
        base.onAttack(other, mainActionPointsCost, bonusActionPointsCost);

        MarkAsAttacking(other);
        this.activeWeapon.onAttack((mech)other);
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

    public void copyFrom(mech original)
    {
        this.baseAtt.setTo(original.baseAtt);

        this.copyAbilitiesFrom(original);

        this.copyBuffsFrom(original);

        this.copyWeaponsFrom(original);

        this.MovementSpeed = original.MovementSpeed;
    }

    public void copyWeaponsFrom(mech m)
    {
        foreach (mechWeapon w in m.weapons)
        {
            mechWeapon newW = w.clone();
            newW.parent = this;
            this.weapons.Add(newW);
        }
    }
}
