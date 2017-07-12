using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Base class for all units in the game.
/// </summary>
public abstract class Unit : unitBase
{
    /////////////////////////////////
    //TBS FRAMEWORK EVENT HANDLERS
    /////////////////////////////////

    /// <summary>
    /// UnitClicked event is invoked when user clicks the unit. It requires a collider on the unit game object to work.
    /// </summary>
    public event EventHandler UnitClicked;
    public event EventHandler UnitRightClicked;
    /// <summary>
    /// UnitSelected event is invoked when user clicks on unit that belongs to him. It requires a collider on the unit game object to work.
    /// </summary>
    public event EventHandler UnitSelected;
    public event EventHandler UnitDeselected;
    /// <summary>
    /// UnitHighlighted event is invoked when user moves cursor over the unit. It requires a collider on the unit game object to work.
    /// </summary>
    public event EventHandler UnitHighlighted;
    public event EventHandler UnitDehighlighted;
    public event EventHandler<AttackEventArgs> UnitAttacked;
    public event EventHandler<AttackEventArgs> UnitDestroyed;
    public event EventHandler<MovementEventArgs> UnitMoved;

    /////////////////////////////////
    //MAP AND UI NEEDED VALUES
    /////////////////////////////////

    public Image healthBar;

    public UnitState UnitState { get; set; }
    public void SetState(UnitState state)
    {
        UnitState.MakeTransition(state);
    }

    /// <summary>
    /// Cell that the unit is currently occupying.
    /// </summary>
    public Cell Cell { get; set; }
    /// <summary>
    /// Determines speed of movement animation.
    /// </summary>
    public float MovementSpeed;
    /// <summary>
    /// Indicates the player that the unit belongs to. Should correspoond with PlayerNumber variable on Player script.
    /// </summary>
    public int PlayerNumber;
    /// <summary>
    /// Indicates if movement animation is playing.
    /// </summary>
    public bool isMoving { get; set; }
    private static IPathfinding _pathfinder = new AStarPathfinding();

    /////////////////////////////////
    /////////GAME SPECIFIC VALUES
    /////////////////////////////////

    /// <summary>
    /// UniTable entry to template from
    /// </summary>
    public attributes dynamicAttributes;
    /// <summary>
    /// List of abilities available to unit
    /// </summary>
    public List<ability> abilities;
    /// <summary>
    /// List of active buffs affecting unit
    /// </summary>
    public List<modifier> buffs;
    /// <summary>
    /// UniTable entry to template from
    /// </summary>
    public String TemplateId;

    public IDictionary<part.slot, part> parts { get; private set; }

    /// <summary>
    /// Method called after object instantiation to initialize fields etc. 
    /// </summary>
    public virtual new void Initialize()
    {
        base.Initialize();
        this.dynamicAttributes = new attributes();
        this.abilities = new List<ability>();
        this.buffs = new List<modifier>();
        this.parts = new Dictionary<part.slot, part>();
    }

    /// <summary>
    /// Method called at start of grid to calculate initial values
    /// </summary>
    public override void GameInit()
    {
        UnitState = new UnitStateNormal(this);
        this.dynamicAttributes = new attributes(this.getSummedAttributes());
        UpdateHpBar();
    }

    /////////////////////////////////
    ////////EVENT CUES
    /////////////////////////////////

    /// <summary>
    /// Method is called at the start of each turn.
    /// </summary>
    public override void onTurnStart()
    {
        foreach(Buff b in this.buffs)
        {
            b.Duration--;
        }
        //recalculate values
        attributes newVals = this.getSummedAttributes();
        //TODO: reassign some values

        this.dynamicAttributes.movementPoints = this.dynamicAttributes.maxMovementPoints;
        this.dynamicAttributes.mainActionPoints = this.dynamicAttributes.maxMainActionPoints;
        this.dynamicAttributes.bonusActionPoints = this.dynamicAttributes.maxBonusActionPoints;

        SetState(new UnitStateMarkedAsFriendly(this));
    }
    /// <summary>
    /// Method is called at the end of each turn.
    /// </summary>
    public override void onTurnEnd()
    {
        SetState(new UnitStateNormal(this));
    }

    /// <summary>
    /// Method is called when units HP drops below 1.
    /// </summary>
    public override void onDeath()
    {
        Cell.IsTaken = false;
        MarkAsDestroyed();
        Destroy(gameObject);
    }

    /// <summary>
    /// Method deals damage to unit given as parameter.
    /// </summary>
    public virtual void onAttack(Unit other, int mainActionPointsCost, int bonusActionPointsCost)
    {
        if (isMoving)
            return;
        if (!parseActionCost(mainActionPointsCost, bonusActionPointsCost))
            return;
        foreach (modifier m in buffs)
        {
            m.onAttack(other);
        }

        MarkAsAttacking(other);
        //do some attacking stuff here

    }
    /// <summary>
    /// Attacking unit calls Defend method on defending unit. 
    /// </summary>
    public virtual void onDefend(Unit other, float damage)
    {
        foreach (modifier m in buffs)
        {
            m.onDefend(other);
        }
        MarkAsDefending(other);

        //defensive parameter parsing
        if(this.dynamicAttributes.shieldPoints > 0)
        {
            float shieldDamage = Math.Min(damage, this.dynamicAttributes.shieldPoints);
            damage -= shieldDamage;
            this.dynamicAttributes.shieldPoints -= shieldDamage;
        }
        this.dynamicAttributes.health -= damage;

        UpdateHpBar();

        if (UnitAttacked != null)
            UnitAttacked.Invoke(this, new AttackEventArgs(other, this, damage));
        //check if dying
        if (this.dynamicAttributes.health <= 0)
        {
            if (UnitDestroyed != null)
                UnitDestroyed.Invoke(this, new AttackEventArgs(other, this, damage));
            onDeath();
        }
    }

    /////////////////////////////////
    ///////GAME UTILITY FUNCTIONS
    /////////////////////////////////

    /// <summary>
    /// Check action cost to see if action is doable
    /// </summary>
    public virtual bool parseActionCost(int mainActionPointsCost, int bonusActionPointsCost)
    {
        if ((mainActionPointsCost > this.dynamicAttributes.mainActionPoints) || (bonusActionPointsCost > this.dynamicAttributes.bonusActionPoints))
            return false;

        this.dynamicAttributes.mainActionPoints -= mainActionPointsCost;
        this.dynamicAttributes.bonusActionPoints -= bonusActionPointsCost;

        if (this.dynamicAttributes.mainActionPoints == 0 && this.dynamicAttributes.bonusActionPoints == 0)
        {
            SetState(new UnitStateMarkedAsFinished(this));
            this.dynamicAttributes.movementPoints = 0;
        }
        return true;
    }

    /// <summary>
    /// Get value of attributes modified by buffs
    /// </summary>
    public virtual attributes getSummedAttributes()
    {
        attributes newAtt = new attributes();
        newAtt.setTo(this.baseAtt);
        foreach (modifier m in this.buffs)
        {
            newAtt.add(m.att);
        }

        return newAtt;
    }

    /// <summary>
    /// Get value of attributes modified by buffs
    /// </summary>
    public void copyBuffsFrom(Unit unit)
    {
        foreach (modifier m in unit.buffs)
        {
            modifier newM = m.clone();
            newM.parent = this;
            this.buffs.Add(newM);
        }
    }

    /// <summary>
    /// Get value of attributes modified by buffs
    /// </summary>
    public void copyAbilitiesFrom(Unit unit)
    {
        foreach (ability a in unit.abilities)
        {
            ability newA = a.clone();
            newA.parent = this;
            this.abilities.Add(newA);
        }
    }

    public virtual bool addPartAs(part part, part.slot slot)
    {
        if (this.parts.ContainsKey(slot) && this.parts[slot] == null)
        {
            this.parts[slot] = part;
            part.setOwner(this);
            return true;
        }
        return false;
    }

    public virtual bool removePart(part part)
    {
        foreach (part.slot s in part.slots)
        {
            if(this.parts[s] == part)
            {
                this.parts[s] = null;
                return true;
            }
        }
        return false;
    }

    /////////////////////////////////
    //////////UI CUES
    /////////////////////////////////

    protected virtual void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && UnitClicked != null)
            UnitClicked.Invoke(this, new EventArgs());
        if (Input.GetMouseButtonDown(1) && UnitRightClicked != null)
            UnitRightClicked.Invoke(this, new EventArgs());
    }
    protected virtual void OnMouseEnter()
    {
        if (UnitHighlighted != null)
            UnitHighlighted.Invoke(this, new EventArgs());
    }
    protected virtual void OnMouseExit()
    {
        if (UnitDehighlighted != null)
            UnitDehighlighted.Invoke(this, new EventArgs());
    }

    /// <summary>
    /// Method is called when unit is selected.
    /// </summary>
    public virtual void OnUnitSelected()
    {
        SetState(new UnitStateMarkedAsSelected(this));
        if (UnitSelected != null)
            UnitSelected.Invoke(this, new EventArgs());
    }
    /// <summary>
    /// Method is called when unit is deselected.
    /// </summary>
    public virtual void OnUnitDeselected()
    {
        SetState(new UnitStateMarkedAsFriendly(this));
        if (UnitDeselected != null)
            UnitDeselected.Invoke(this, new EventArgs());
    }

    private void UpdateHpBar()
    {
            if (healthBar != null)
                healthBar.transform.localScale = new Vector3((float)(this.dynamicAttributes.health / this.dynamicAttributes.maxHealth), 1, 1);
           // GetComponentInChildren<Image>().color = Color.Lerp(Color.red, Color.green,
               // (float)(this.dynamicAttributes.health / this.dynamicAttributes.maxHealth));
    }

    public virtual void Move(Cell destinationCell, List<Cell> path)
    {
        if (isMoving)
            return;

        var totalMovementCost = path.Sum(h => h.MovementCost);
        if (this.dynamicAttributes.movementPoints < totalMovementCost)
            return;

        this.dynamicAttributes.movementPoints -= totalMovementCost;

        Cell.IsTaken = false;
        Cell = destinationCell;
        destinationCell.IsTaken = true;

        if (MovementSpeed > 0)
            StartCoroutine(MovementAnimation(path));
        else
            transform.position = Cell.transform.position;

        if (UnitMoved != null)
            UnitMoved.Invoke(this, new MovementEventArgs(Cell, destinationCell, path));    
    }

    protected virtual IEnumerator MovementAnimation(List<Cell> path)
    {
        isMoving = true;
        path.Reverse();
        foreach (var cell in path)
        {
            while (new Vector2(transform.position.x,transform.position.y) != new Vector2(cell.transform.position.x,cell.transform.position.y))
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(cell.transform.position.x,cell.transform.position.y,transform.position.z), Time.deltaTime * MovementSpeed);
                yield return 0;
            }
        }

        isMoving = false;
    }

    ///<summary>
    /// Method indicates if unit is capable of moving to cell given as parameter.
    /// </summary>
    public virtual bool IsCellMovableTo(Cell cell)
    {
        return !cell.IsTaken;
    }
    /// <summary>
    /// Method indicates if unit is capable of moving through cell given as parameter.
    /// </summary>
    public virtual bool IsCellTraversable(Cell cell)
    {
        return !cell.IsTaken;
    }
    /// <summary>
    /// Method returns all cells that the unit is capable of moving to.
    /// </summary>
    public List<Cell> GetAvailableDestinations(List<Cell> cells)
    {
        var ret = new List<Cell>();
        var cellsInMovementRange = cells.FindAll(c => IsCellMovableTo(c) && c.GetDistance(Cell) <= this.dynamicAttributes.movementPoints);

        var traversableCells = cells.FindAll(c => IsCellTraversable(c) && c.GetDistance(Cell) <= this.dynamicAttributes.movementPoints);
        traversableCells.Add(Cell);

        foreach (var cellInRange in cellsInMovementRange)
        {
            if (cellInRange.Equals(Cell)) continue;

            var path = FindPath(traversableCells, cellInRange);
            var pathCost = path.Sum(c => c.MovementCost);
            if (pathCost > 0 && pathCost <= this.dynamicAttributes.movementPoints)
                ret.AddRange(path);
        }
        return ret.FindAll(IsCellMovableTo).Distinct().ToList();
    }

    public List<Cell> FindPath(List<Cell> cells, Cell destination)
    {
        return _pathfinder.FindPath(GetGraphEdges(cells), Cell, destination);
    }

    /// <summary>
    /// Method returns graph representation of cell grid for pathfinding.
    /// </summary>
    protected virtual Dictionary<Cell, Dictionary<Cell, int>> GetGraphEdges(List<Cell> cells)
    {
        Dictionary<Cell, Dictionary<Cell, int>> ret = new Dictionary<Cell, Dictionary<Cell, int>>();
        foreach (var cell in cells)
        {
            if (IsCellTraversable(cell) || cell.Equals(Cell))
            {
                ret[cell] = new Dictionary<Cell, int>();
                foreach (var neighbour in cell.GetNeighbours(cells).FindAll(IsCellTraversable))
                {
                    ret[cell][neighbour] = neighbour.MovementCost;
                }
            }
        }
        return ret;
    }

    /// <summary>
    /// Gives visual indication that the unit is under attack.
    /// </summary>
    /// <param name="other"></param>
    public abstract void MarkAsDefending(Unit other);
    /// <summary>
    /// Gives visual indication that the unit is attacking.
    /// </summary>
    /// <param name="other"></param>
    public abstract void MarkAsAttacking(Unit other);
    /// <summary>
    /// Gives visual indication that the unit is destroyed. It gets called right before the unit game object is
    /// destroyed, so either instantiate some new object to indicate destruction or redesign Defend method. 
    /// </summary>
    public abstract void MarkAsDestroyed();

    /// <summary>
    /// Method marks unit as current players unit.
    /// </summary>
    public abstract void MarkAsFriendly();
    /// <summary>
    /// Method mark units to indicate user that the unit is in range and can be attacked.
    /// </summary>
    public abstract void MarkAsReachableEnemy();
    /// <summary>
    /// Method marks unit as currently selected, to distinguish it from other units.
    /// </summary>
    public abstract void MarkAsSelected();
    /// <summary>
    /// Method marks unit to indicate user that he can't do anything more with it this turn.
    /// </summary>
    public abstract void MarkAsFinished();
    /// <summary>
    /// Method returns the unit to its base appearance
    /// </summary>
    public abstract void UnMark();

    public IEnumerator Jerk(Unit other, float movementTime)
    {
        var heading = other.transform.position - transform.position;
        var direction = heading / heading.magnitude;
        float startTime = Time.time;

        while (true)
        {
            var currentTime = Time.time;
            if (startTime + movementTime < currentTime)
                break;
            transform.position = Vector3.Lerp(transform.position, transform.position + (direction / 2.5f), ((startTime + movementTime) - currentTime));
            yield return 0;
        }
        startTime = Time.time;
        while (true)
        {
            var currentTime = Time.time;
            if (startTime + movementTime < currentTime)
                break;
            transform.position = Vector3.Lerp(transform.position, transform.position - (direction / 2.5f), ((startTime + movementTime) - currentTime));
            yield return 0;
        }
        transform.position = Cell.transform.position + new Vector3(0, 0, -1);
    }

    public IEnumerator Glow(Color color, float cooloutTime)
    {
        var _renderer = GetComponent<SpriteRenderer>();
        float startTime = Time.time;

        while (true)
        {
            var currentTime = Time.time;
            if (startTime + cooloutTime < currentTime)
                break;

            _renderer.color = Color.Lerp(Color.white, color, (startTime + cooloutTime) - currentTime);
            yield return 0;
        }

        _renderer.color = Color.white;
    }

    public IEnumerator Pulse(float breakTime, float delay, float scaleFactor)
    {
        var baseScale = transform.localScale;
        while (true)
        {
            float time1 = Time.time;
            while (time1 + delay > Time.time)
            {
                transform.localScale = Vector3.Lerp(baseScale * scaleFactor, baseScale, (time1 + delay) - Time.time);
                yield return 0;
            }

            float time2 = Time.time;
            while (time2 + delay > Time.time)
            {
                transform.localScale = Vector3.Lerp(baseScale, baseScale * scaleFactor, (time2 + delay) - Time.time);
                yield return 0;
            }

            yield return new WaitForSeconds(breakTime);
        }
    }


}

public class MovementEventArgs : EventArgs
{
    public Cell OriginCell;
    public Cell DestinationCell;
    public List<Cell> Path;

    public MovementEventArgs(Cell sourceCell, Cell destinationCell, List<Cell> path)
    {
        OriginCell = sourceCell;
        DestinationCell = destinationCell;
        Path = path;
    }
}
public class AttackEventArgs : EventArgs
{
    public Unit Attacker;
    public Unit Defender;

    public float Damage;

    public AttackEventArgs(Unit attacker, Unit defender, float damage)
    {
        Attacker = attacker;
        Defender = defender;

        Damage = damage;
    }
}
