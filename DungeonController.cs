using System;
using System.Linq;
using UnityEngine;

public class DungeonController : MonoBehaviour
{
    [SerializeField]
    private int _height, _width, _rooms, _seed;
    private Dungeon _dungeon;
    private void Start()
    {
       _dungeon = new Dungeon(_height, _width, _rooms, _seed);
    }
    [ContextMenu("Перестроить подземелье")]
    void UpdateDungeon()
    {
        _dungeon.ChangeData(_height, _width, _rooms, _seed);
        _dungeon.BuildDungeonScheme();
    }
 
}
[System.Serializable]
public class Dungeon
{
    private int _width, _height;

    string[,] _dungeonScheme;
    private RoomCoordinate[] _rooms;
    private RoomCoordinate _dungeonStart;

    private int _contentLootCount;
    private int _seed;

    private System.Random _dungeonRandomized;
    public Dungeon(int width, int height, int contentLootCount, int seed = 0)
    {
        _width = width;
        _height = height;
        _contentLootCount = contentLootCount;
        _seed = seed;

        BuildDungeonScheme();
    }

    public void ChangeData(int width, int height, int contentLootCount, int seed = 0)
    {
        _width = width;
        _height = height;
        _contentLootCount = contentLootCount;
        _seed = seed;
    }
    public void BuildDungeonScheme()
    {
        if (_seed != 0)
            _dungeonRandomized = new System.Random(_seed);
        else
            _dungeonRandomized = new System.Random();


        _dungeonScheme = new string[_height,_width];
        _rooms = new RoomCoordinate[_contentLootCount];

        FillContentRooms();
        FillStartRoom();
        PrintDungeon();
    }

    private void FillStartRoom()
    {
        int randomX = _dungeonRandomized.Next(1, _width - 2);
        int randomY = _dungeonRandomized.Next(1, _height - 2);
        RoomCoordinate startCord = new RoomCoordinate(randomX,randomY);

        while (_rooms.Contains(startCord))
        {
            randomX = _dungeonRandomized.Next(1, _width - 2);
            randomY = _dungeonRandomized.Next(1, _width - 2);
            startCord.Set(randomX, randomY);
        }
        _dungeonScheme[randomY, randomX] = "S";
        _dungeonStart = startCord;
    }

    private void FillContentRooms()
    {
        for (int i = 0; i < _contentLootCount; i++)
        {
            int randomX = _dungeonRandomized.Next(_width - 1);
            int randomY = _dungeonRandomized.Next(_height - 1);
            _dungeonScheme[randomY, randomX] = "C";
            _rooms[i] = new RoomCoordinate(randomX, randomY);
        }
    }
    private void PrintDungeon()
    {
        string dungeonLine = "";
        for (int i = 0; i < _height; i++)
        {
            for(int j = 0; j < _width; j++)
            {
                if (_dungeonScheme[i, j] == null)
                    dungeonLine += "#";
                else
                    dungeonLine += _dungeonScheme[i, j];
            }
            dungeonLine += "\n";
        }
        Debug.Log(dungeonLine);
    }

    private struct RoomCoordinate
    {
        public int X;
        public int Y;

        public RoomCoordinate(int x,int y)
        {
            X = x;
            Y = y;
        }
        public void Set(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}