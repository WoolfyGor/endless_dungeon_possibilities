using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class DungeonController : MonoBehaviour
{
    [SerializeField]
    private int _height, _width, _rooms, _seed;
    [SerializeField]
    private Dungeon _dungeon;

    List<GameObject> _spawnedTiles;
    [SerializeField]
    DungeonTile _tilePrefab;
    private void Start()
    {
       _dungeon = new Dungeon(_height, _width, _rooms, _seed);
        SpawnDungeon();
    }
    [ContextMenu("ѕерестроить схему подземель€")]
    void UpdateDungeon()
    {
        _dungeon.ChangeData(_height, _width, _rooms, _seed);
        _dungeon.BuildDungeonScheme();
    }
    [ContextMenu("ѕерестроить модели подземель€")]
    void SpawnDungeon()
    {
        ClearDungeon();
        _spawnedTiles = new();
        var schema = _dungeon.GetDungeonScheme();
        var tiles = _dungeon.GetDungeonTiles();
        foreach(var tile in tiles)
        {
            var g = Instantiate(_tilePrefab, transform);
            g.SetupTile(tile, schema);
            _spawnedTiles.Add(g.gameObject);
        }
    }
    void ClearDungeon()
    {
        if (_spawnedTiles == null || _spawnedTiles.Count == 0)
            return;
        foreach(var tile in _spawnedTiles)
        {
            Destroy(tile);
        }
        _spawnedTiles.Clear();
    }
}

[System.Serializable]
public class Dungeon
{
    private int _width, _height;

    string[,] _dungeonScheme;
    [SerializeField]
    private RoomCoordinate[] _rooms;
    [SerializeField]
    private RoomCoordinate _dungeonStart;
    [SerializeField]
    private List<RoomCoordinate> _hallways;
    
    private int _contentLootCount;
    private int _seed;

    private System.Random _dungeonRandomized;
    private string _dungeonMap;
    public Dungeon(int width, int height, int contentLootCount, int seed = 0)
    {
        _width = width;
        _height = height;
        _contentLootCount = contentLootCount;
        _seed = seed;

        BuildDungeonScheme();
    }
    public string[,] GetDungeonScheme()
    {
        return _dungeonScheme;
    }
    public RoomCoordinate[] GetDungeonTiles()
    {
        var rooms = new List<RoomCoordinate>();
        rooms.AddRange(_rooms);
        rooms.AddRange(_hallways);
        rooms.Add(_dungeonStart);
        
        return rooms.ToArray();
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
        ConnectStartToContent();
        FillSidePassages();
        FillEmptyTiles();
        PrintDungeon();
    }

    private void FillEmptyTiles()
    {
        string dungeonLine = "";
        for (int i = 0; i < _height; i++)
        {
            for (int j = 0; j < _width; j++)
            {
                if (_dungeonScheme[i, j] == null) { 
                    dungeonLine += "#";
                    _dungeonScheme[i, j] = "#";
                }
                else
                    dungeonLine += _dungeonScheme[i, j];
            }
            dungeonLine += "\n";
        }
        _dungeonMap = dungeonLine;
    }
    private void FillSidePassages()
    {
        for (int y = 1; y < _height - 1; y++)
        {
            for (int x = 1; x < _width - 1; x++)
            {
                if (_dungeonScheme[y, x] == ".")
                {
                    TryAddSidePassage(x, y);
                }
            }
        }
    }
    private void TryAddSidePassage(int x, int y)
    {

        (int dx, int dy)[] directions = new (int, int)[]
        {
        (1, 0), (-1, 0), (0, 1), (0, -1)
        };

        foreach (var (dx, dy) in directions)
        {
            int nx = x + dx;
            int ny = y + dy;

            if (_dungeonScheme[ny, nx] == null && _dungeonRandomized.NextDouble() < 0.2)
            {
                _dungeonScheme[ny, nx] = ".";
                _hallways.Add(new RoomCoordinate(nx, ny));
            }
        }
    }
    private void FillStartRoom()
    {
        bool isPlaced = false;
        while (!isPlaced)
        {
            int randomX = _dungeonRandomized.Next(1, _width - 2);
            int randomY = _dungeonRandomized.Next(1, _height - 2);
            RoomCoordinate candidate = new RoomCoordinate(randomX, randomY);

            bool nearContent = false;
            for (int i = 0; i < _rooms.Length; i++)
            {
                if (_rooms[i].X == 0 && _rooms[i].Y == 0)
                    continue;

                int dx = Math.Abs(candidate.X - _rooms[i].X);
                int dy = Math.Abs(candidate.Y - _rooms[i].Y);
                int chebDistance = Math.Max(dx, dy);
                if (chebDistance <= 1)
                {
                    nearContent = true;
                    break;
                }
            }

            if (nearContent)
                continue;

            _dungeonScheme[randomY, randomX] = "S";
            _dungeonStart = candidate;
            isPlaced = true;
        }
    }


    private void FillContentRooms()
    {
        for (int i = 0; i < _contentLootCount; i++)
        {
            bool isPlaced = false;
            while (!isPlaced)
            {
                // ћожно избегать крайних €чеек, если не нужны комнаты на границе
                int randomX = _dungeonRandomized.Next(1, _width - 1);
                int randomY = _dungeonRandomized.Next(1, _height - 1);

                // ≈сли €чейка уже зан€та (например, коридором, стеной или другой комнатой), пропускаем
                if (_dungeonScheme[randomY, randomX] != null)
                    continue;

                RoomCoordinate candidate = new RoomCoordinate(randomX, randomY);

                // ≈сли это перва€ комната, размещаем еЄ без дополнительных проверок
                if (i == 0)
                {
                    _dungeonScheme[randomY, randomX] = "C";
                    _rooms[i] = candidate;
                    isPlaced = true;
                }
                else
                {
                    bool adjacentViolation = false; // комната слишком близко (1 €чейка)
                    bool withinRange = false;        // хот€ бы одна комната находитс€ в пределах 5 €чеек

                    // ѕровер€ем все уже размещенные комнаты
                    for (int j = 0; j < i; j++)
                    {
                        int dx = Math.Abs(candidate.X - _rooms[j].X);
                        int dy = Math.Abs(candidate.Y - _rooms[j].Y);
                        int chebDistance = Math.Max(dx, dy);

                        // ≈сли комната находитс€ р€дом (включа€ диагональ), нельз€ ставить candidate
                        if (chebDistance <= 1)
                        {
                            adjacentViolation = true;
                            break;
                        }
                        // ≈сли хоть одна комната расположена на рассто€нии не более 5, значит кандидат прив€зан к кластеру
                        if (chebDistance <= 5)
                        {
                            withinRange = true;
                        }
                    }

                    // ≈сли кандидат удовлетвор€ет услови€м, размещаем его:
                    // - Ќе должно быть соседей (adjacentViolation == false)
                    // - » хот€ бы одна комната из уже поставленных находитс€ в пределах 5 €чеек (withinRange == true)
                    if (!adjacentViolation && withinRange)
                    {
                        _dungeonScheme[randomY, randomX] = "C";
                        _rooms[i] = candidate;
                        isPlaced = true;
                    }
                }
            }
        }
    }


    void ConnectStartToContent()
    {
        _hallways = new();
        foreach (var room in _rooms)
        {
            int currentX = _dungeonStart.X;
            int currentY = _dungeonStart.Y;

            while (currentX != room.X)
            {
                currentX += (room.X > currentX) ? 1 : -1;
                if (_dungeonScheme[currentY, currentX] == null) { 
                    _dungeonScheme[currentY, currentX] = ".";
                    _hallways.Add(new RoomCoordinate(currentX, currentY));
                }

            }
            while (currentY != room.Y)
            {
                currentY += (room.Y > currentY) ? 1 : -1;
                if (_dungeonScheme[currentY, currentX] == null)
                {
                    _dungeonScheme[currentY, currentX] = ".";
                    _hallways.Add(new RoomCoordinate(currentX,currentY));
                }
            }
        }
    }
    private void PrintDungeon()
    {
        Debug.Log(_dungeonMap);
    }
    [System.Serializable]
    public struct RoomCoordinate
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
        public override bool Equals(object obj)
        {
            if (!(obj is RoomCoordinate))
                return false;

            RoomCoordinate other = (RoomCoordinate)obj;
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return X * 397 ^ Y; // Somehash
        }
    }
}