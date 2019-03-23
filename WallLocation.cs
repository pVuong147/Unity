using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct WallLocation {

    public int row { get; private set; }
    public int column { get; private set; }
    public Vector3 position { get; private set; }
    public Quaternion rotation { get; private set; }

    public WallLocation(int row, int col, Vector3 pos, Quaternion rot)
    {
        this.row = row;
        column = col;
        position = pos;
        rotation = rot;
    }
}
