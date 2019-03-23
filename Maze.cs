using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Maze : NetworkBehaviour {

    public GameObject maze;
    public GameObject wall;

    [SerializeField]
    private int floorX;
    [SerializeField]
    private int floorZ;
    [SerializeField]
    private int xLen;
    [SerializeField]
    private int zLen;
    [SerializeField]    
    private float wallHeight;
    private float n;
    private int k;
    private Vector3 initPos;

    private Cell[,] cells;
    private int totalCells;
    private bool mazeReady;
    private List<GameObject> wallsToSpawn;
    private List<string> wallsToRemove;

    // Use this for initialization
    void Start () {

        n = Mathf.Round((float)floorX / xLen);
        k = (int)n;
        totalCells = k * k;
        cells = new Cell[k, k];
        for (int i = 0; i < k; i++)
            for (int j = 0; j < k; j++)
                cells[i, j] = new Cell(i, j);
        mazeReady = false;
        wallsToSpawn = new List<GameObject>();
        wallsToRemove = new List<string>();
    }
	
    [Command]
    private void CmdCreateWalls()
    {
        CreateWalls();
    }
    private void CreateWalls()
    {
        GameObject mazeObject = (GameObject)Instantiate(maze);

        initPos = new Vector3(-floorX / 2 + xLen / 2, 0, -floorZ / 2);
        Vector3 pos;

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n - 1; j++)
            {
                pos = new Vector3(initPos.x + j * xLen, 0, initPos.z + i * xLen);
                WallLocation wallLocation = new WallLocation(i, j, pos, Quaternion.identity);
                InstantiateWall(mazeObject, wallLocation, i, true);
            }
        }

        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n; j++)
            {
                pos = new Vector3(initPos.x + (j * xLen) - xLen / 2, 0, initPos.z + i * xLen + xLen / 2);
                WallLocation wallLocation = new WallLocation(i, j, pos, Quaternion.Euler(0, 90, 0));
                InstantiateWall(mazeObject, wallLocation, j, false);
            }
        }
    }
    private void InstantiateWall(GameObject mazeObject, WallLocation wallLocation, int checkingIndex, bool checkingRow)
    {
        GameObject tmpWall = Instantiate(this.wall, wallLocation.position, wallLocation.rotation) as GameObject;
        tmpWall.transform.parent = mazeObject.transform;
        tmpWall.name = "Wall " + wallLocation.row + ", " + wallLocation.column;
        Wall wall = tmpWall.GetComponent<Wall>();
        wall.SetLocation(wallLocation);
        if (checkingIndex > 0 && checkingIndex < n - 1)
        {
            wall.SetRemovable();
            if (checkingRow)
            {
                tmpWall.name += ", removable";
                cells[wallLocation.row - 1, wallLocation.column].AddWall(tmpWall);
            }
            else
            {
                tmpWall.name += ", removable 90";
                cells[wallLocation.row, wallLocation.column - 1].AddWall(tmpWall);
            }
            cells[wallLocation.row, wallLocation.column].AddWall(tmpWall);
        }
    }
    [Command]
    private void CmdFindNeighbors()
    {
        FindNeighbors();
    }
    private void FindNeighbors()
    {
        foreach (Cell cell in cells)
        {
            if (cell.rowNumber > 0)
                cell.AddNeighbor(cells[cell.rowNumber - 1, cell.columnNumber]);
            if (cell.columnNumber > 0)
                cell.AddNeighbor(cells[cell.rowNumber, cell.columnNumber - 1]);
            if (cell.columnNumber < k - 1)
                cell.AddNeighbor(cells[cell.rowNumber, cell.columnNumber + 1]);
            if (cell.rowNumber < k - 1)
                cell.AddNeighbor(cells[cell.rowNumber + 1, cell.columnNumber]);
        }
    }
    [Command]
    private void CmdCreateMaze()
    {
        CreateMaze();
    }
    // This method creates a maze with the DFS algorithm
    private void CreateMaze()
    {
        Stack<Cell> visitedCells = new Stack<Cell>();
        int visited = 0;
        int currentX = 0, currentY = 0;

        // choose a starting cell at random
        currentX = MyRandom.GiveRandom(0, k);
        currentY = MyRandom.GiveRandom(0, k);
        visited++;

        while (visited < totalCells)
        {
            // mark the current cell as visited
            cells[currentX, currentY].Visit();
            if (cells[currentX, currentY].NeighborsToVisit())
            {
                // pick a random neighbor cell
                Cell neighbor = cells[currentX, currentY].PickNeighbor();
                // find a wall to remove between the current cell and its neighbor
                GameObject wallToRemove = cells[currentX, currentY].FindSeparatingWall(neighbor);
                if (wallToRemove != null)
                {
                    wallsToSpawn.Add(wallToRemove);
                    wallsToRemove.Add(wallToRemove.name);
                    RemoveWall(wallToRemove, wallHeight);
                }
                visitedCells.Push(cells[currentX, currentY]);
                // set the current cell to the chosen neighbor
                currentX = neighbor.rowNumber;
                currentY = neighbor.columnNumber;
                visited++;
            }
            else
            {
                // if there are no neighbor cells to visit, go back to the previously visited cell
                Cell lastCell = visitedCells.Pop();
                currentX = lastCell.rowNumber;
                currentY = lastCell.columnNumber;
            }
        }

        mazeReady = true;
    }
}
