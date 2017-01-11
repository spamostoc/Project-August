using System.Collections.Generic;
using System.Linq;

class CellGridStateUnitSelected : CellGridState
{
    private Unit _unit;
    private List<Cell> _pathsInRange;
    private List<Unit> _unitsInRange;

    private Cell _unitCell;

    public CellGridStateUnitSelected(CellGrid cellGrid, Unit unit) : base(cellGrid)
    {
        _unit = unit;
        _pathsInRange = new List<Cell>();
        _unitsInRange = new List<Unit>();
    }

    public override void OnCellClicked(Cell cell)
    {
        if (_unit.isMoving)
            return;
        if (cell.IsTaken)
        {
            _cellGrid.CellGridState = new CellGridStateWaitingForInput(_cellGrid);
            return;
        }

        if (!_pathsInRange.Contains(cell))
        {
            _cellGrid.CellGridState = new CellGridStateWaitingForInput(_cellGrid);
        }
    }

    public override void OnCellRightClicked(Cell cell)
    {
        if (!_unit.isMoving && !cell.IsTaken && _pathsInRange.Contains(cell))
        {
            var path = _unit.FindPath(_cellGrid.Cells, cell);
            _unit.Move(cell, path);
            _cellGrid.CellGridState = new CellGridStateUnitSelected(_cellGrid, _unit);
        }
    }

    public override void OnUnitClicked(Unit unit)
    {
        if (unit.Equals(_unit) || unit.isMoving)
            return;

        //what the heck does any of this even do
        if (_unitsInRange.Contains(unit) &&
            (_unit.dynamicAttributes.mainActionPoints > 0 || _unit.dynamicAttributes.bonusActionPoints > 0))
        {
            //_unit.onAttack(unit, 1);
            //we just return to the same state and do nothing?
            _cellGrid.CellGridState = new CellGridStateUnitSelected(_cellGrid, _unit);
        }

        if (unit.PlayerNumber.Equals(_unit.PlayerNumber))
        {
            _cellGrid.CellGridState = new CellGridStateUnitSelected(_cellGrid, unit);
        }

    }

    public override void OnCellDeselected(Cell cell)
    {
        base.OnCellDeselected(cell);

        foreach (var _cell in _pathsInRange)
        {
            _cell.MarkAsReachable();
        }
        foreach (var _cell in _cellGrid.Cells.Except(_pathsInRange))
        {
            _cell.UnMark();
        }
    }

    public override void OnCellSelected(Cell cell)
    {
        base.OnCellSelected(cell);
        if (!_pathsInRange.Contains(cell)) return;
        var path = _unit.FindPath(_cellGrid.Cells, cell);
        foreach (var _cell in path)
        {
            _cell.MarkAsPath();
        }
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();

        _unit.OnUnitSelected();
        _unitCell = _unit.Cell;

        _pathsInRange = _unit.GetAvailableDestinations(_cellGrid.Cells);
        var cellsNotInRange = _cellGrid.Cells.Except(_pathsInRange);

        foreach (var cell in cellsNotInRange)
        {
            cell.UnMark();
        }
        foreach (var cell in _pathsInRange)
        {
            cell.MarkAsReachable();
        }
        //this line checks if we have points left, if we do skip the rest of the function which will end the state
        if (_unit.dynamicAttributes.mainActionPoints <= 0 && _unit.dynamicAttributes.bonusActionPoints <= 0) return;

        //change all this to highlight in ability state only
        /*foreach (var currentUnit in _cellGrid.Units)
        {
            if (currentUnit.PlayerNumber.Equals(_unit.PlayerNumber))
                continue;

            if (_unit.isUnitReachable(currentUnit, _unit.AttackRange, _unit.Cell))
            {
                currentUnit.SetState(new UnitStateMarkedAsReachableEnemy(currentUnit));
                _unitsInRange.Add(currentUnit);
            }
        }*/

        if (_unitCell.GetNeighbours(_cellGrid.Cells).FindAll(c => c.MovementCost <= _unit.dynamicAttributes.movementPoints).Count == 0
            && _unitsInRange.Count == 0)
            _unit.SetState(new UnitStateMarkedAsFinished(_unit));
    }

    public override void OnStateExit()
    {
        _unit.OnUnitDeselected();
        foreach (var unit in _unitsInRange)
        {
            if (unit == null) continue;
            unit.SetState(new UnitStateNormal(unit));
        }
        foreach (var cell in _cellGrid.Cells)
        {
            cell.UnMark();
        }
    }

    public void OnAbilityActive(int index)
    {
        if (index < _unit.abilities.Count)
        _cellGrid.CellGridState = new CellGridStateAbilityActive(_cellGrid, _unit, _unit.abilities[index]);
    }
}

