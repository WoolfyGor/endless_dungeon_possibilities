using System.Collections.Generic;
using UnityEngine;
public class DungeonTile : MonoBehaviour
{
    [SerializeField] GameObject _northWall;
    [SerializeField] GameObject _southWall;
    [SerializeField] GameObject _eastWall;
    [SerializeField] GameObject _westWall;
    [SerializeField] GameObject _floor;
    [SerializeField] GameObject _contentRoom;

    [SerializeField]
    public Dungeon.RoomCoordinate coordinate;

    string[,] _dungeonScheme;
    [SerializeField]
    List<GameObject> FloorSpawnParts,FloorHallwayParts;
    
    public void SetFloor(string type)
    {
        Color color = Color.white;
        var rnd = Dungeon.dungeonRandomized;
        switch (type)
        {
            case ".":
                color = Color.red;
                if (rnd.NextDouble() > 0.5)
                    break;
                var g = FloorHallwayParts[rnd.Next(0, FloorHallwayParts.Count - 1)];
                g.SetActive(true);
                int randomRotationMultiplier = rnd.Next(0, 3);
                g.transform.Rotate(0, 90 * randomRotationMultiplier, 0);
                break;

            case "C":
                color = Color.blue;
                _contentRoom.SetActive(true);
                break;

            case "S":
                color  = Color.green;
                FloorSpawnParts[rnd.Next(0, FloorSpawnParts.Count - 1)].SetActive(true);
                break;
        }

        _floor.GetComponent<MeshRenderer>().material.color = color;

    }
    public void SetupTile(Dungeon.RoomCoordinate cord, string[,] schema)
    {
        _dungeonScheme = schema;
        coordinate = cord;
        Vector3 tilePosition = new Vector3(coordinate.X * 3, 0, -coordinate.Y * 3);
        transform.localPosition = tilePosition;

        SetFloor(_dungeonScheme[coordinate.Y, coordinate.X]);

        SetWall("North", !HasNeighbor(coordinate.X, coordinate.Y, 0, -1));
        SetWall("South", !HasNeighbor(coordinate.X, coordinate.Y, 0, 1));
        SetWall("West", !HasNeighbor(coordinate.X, coordinate.Y, -1, 0));
        SetWall("East", !HasNeighbor(coordinate.X, coordinate.Y, 1, 0));
    }

    bool HasNeighbor(int x, int y, int dx, int dy)
    {
        int nx = x + dx;
        int ny = y + dy;
        if (nx < 0 || ny < 0 || nx >= _dungeonScheme.GetUpperBound(1) || ny >= _dungeonScheme.GetUpperBound(0)) return false;
        return _dungeonScheme[ny, nx] != "#" && _dungeonScheme[ny, nx] != null;
    }
    public void SetWall(string direction, bool active)
    {
        switch (direction)
        {
            case "North": _northWall.SetActive(active); break;
            case "South": _southWall.SetActive(active); break;
            case "East": _eastWall.SetActive(active); break;
            case "West": _westWall.SetActive(active); break;
        }
    }

  
}
