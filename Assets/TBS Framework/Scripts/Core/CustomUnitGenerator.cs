using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomUnitGenerator : MonoBehaviour, IUnitGenerator
{
    public Transform UnitsParent;
    public Transform CellsParent;
    public Transform Alien4;
    public Transform PlayerSpawns;

    /// <summary>
    /// Returns units that are already children of UnitsParent object.
    /// </summary>
    public List<Unit> SpawnUnits(List<Cell> cells)
    {
        List<Unit> ret = new List<Unit>();

        if (pManager.pDataManager.playerMechs != null && pManager.pDataManager.playerMechs.Count > 0)
        {
            spawnPlayerUnits();
        }

        //spawn from script here
        for (int i = 0; i < UnitsParent.childCount; i++)
        {
            var unit = UnitsParent.GetChild(i).GetComponent<Unit>();
            if (unit != null)
            {
                var cell = cells.OrderBy(h => Math.Abs((h.transform.position - unit.transform.position).magnitude)).First();
                if (!cell.IsTaken)
                {
                    cell.IsTaken = true;
                    unit.Cell = cell;
                    unit.transform.position = cell.transform.position;
                    unit.Initialize();
                    ret.Add(unit);
                }//Unit gets snapped to the nearest cell
                else
                {
                    Destroy(unit.gameObject);
                }//If the nearest cell is taken, the unit gets destroyed.
            }
            else
            {
                Debug.LogError("Invalid object in Units Parent game object");
            }

        }
        return ret;
    }

    public void SnapToGrid()
    {
        List<Transform> cells = new List<Transform>();

        foreach (Transform cell in CellsParent)
        {
            cells.Add(cell);
        }

        foreach (Transform unit in UnitsParent)
        {
            var closestCell = cells.OrderBy(h => Math.Abs((h.transform.position - unit.transform.position).magnitude)).First();
            if (!closestCell.GetComponent<Cell>().IsTaken)
            {
                Vector3 offset = new Vector3(0, 0, closestCell.GetComponent<Cell>().GetCellDimensions().z);
                unit.position = closestCell.transform.position - offset;
            }//Unit gets snapped to the nearest cell
        }

        //snap spawn markers for player units
        foreach (Transform spawnPoint in PlayerSpawns)
        {
            var closestCell = cells.OrderBy(h => Math.Abs((h.transform.position - spawnPoint.transform.position).magnitude)).First();
            if (!closestCell.GetComponent<Cell>().IsTaken)
            {
                Vector3 offset = new Vector3(0, 0, closestCell.GetComponent<Cell>().GetCellDimensions().z);
                spawnPoint.position = closestCell.transform.position - offset;
            }

        }
    }

    private void spawnPlayerUnits()
    {
        if (pManager.pDataManager.playerMechs.Count > PlayerSpawns.childCount)
        {
            throw new IndexOutOfRangeException("too many mechs or too few spawn points");
        }

        for (int index = 0; index < PlayerSpawns.childCount; index++)
        {
            mech mechTemplate = pManager.pDataManager.playerMechs[index];
            //need to alias all the player classes to prefabs
            //consider having the prefabs override the save files every time on max values?
            Debug.Log(mechTemplate.GetType());
            Debug.Log(UniTable.prefabTable);
            Transform ret = (Transform)Instantiate(UniTable.prefabTable[mechTemplate.GetType()], PlayerSpawns.GetChild(index).position , Quaternion.identity, UnitsParent);
            if (pManager.pDataManager.playerMechs.Count > 0)
            {
                ret.GetComponent<mech>().copyFrom(mechTemplate);
            }
        }
    }
}

