using UnityEngine;
public class DungeonTile : MonoBehaviour
{
    [SerializeField] GameObject NorthWall;
    [SerializeField] GameObject SouthWall;
    [SerializeField] GameObject EastWall;
    [SerializeField] GameObject WestWall;
    [SerializeField] GameObject Floor;

    [SerializeField]
    public Dungeon.RoomCoordinate coordinate;

    string[,] _dungeonScheme;

    public void SetFloor(string type)
    {
        if(type == ".")
            Floor.GetComponent<MeshRenderer>().material.color = Color.red;
        if (type == "C")
            Floor.GetComponent<MeshRenderer>().material.color = Color.blue;
        if (type == "S")
            Floor.GetComponent<MeshRenderer>().material.color = Color.green;
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
            case "North": NorthWall.SetActive(active); break;
            case "South": SouthWall.SetActive(active); break;
            case "East": EastWall.SetActive(active); break;
            case "West": WestWall.SetActive(active); break;
        }
    }

  
}
