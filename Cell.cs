using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell {

    private bool visited;
    public int rowNumber { get; private set; }
    public int columnNumber { get; private set; }
    private List<GameObject> walls;
    private List<Cell> neighbors;
    private List<Cell> toVisit;    

    public Cell()
    {
        walls = new List<GameObject>();
        neighbors = new List<Cell>();
        toVisit = new List<Cell>();
    }
    public Cell(int row, int column)
    {
        rowNumber = row;
        columnNumber = column;
        walls = new List<GameObject>();
        neighbors = new List<Cell>();
        toVisit = new List<Cell>();
    }
    public void Visit()
    {
        visited = true;
    }
    public void AddWall(GameObject wallGO)
    {
        walls.Add(wallGO);
    }
    public void AddNeighbor(Cell cell)
    {
        neighbors.Add(cell);
    }
    public bool NeighborsToVisit()
    {
        toVisit = new List<Cell>();
        foreach (Cell c in neighbors)
        {
            if (!c.visited)
                toVisit.Add(c);
        }
        if (toVisit.Count > 0)
            return true;

        return false;
    }
    public Cell PickNeighbor()
    {
        int random = MyRandom.GiveRandom(0, toVisit.Count);
        return toVisit[random];
    }
    public GameObject FindSeparatingWall(Cell neighbor)
    {
        foreach (GameObject wall in walls)
        {
            if (FoundElement(neighbor, wall.name))
                return wall;
        }
        return null;
    }
    private bool FoundElement(Cell cell, string name)
    {
        foreach (GameObject wall in cell.walls)
        {
            if (cell.walls.Exists(x => x.name == name))
                return true;
        }
        return false;
    }
}
