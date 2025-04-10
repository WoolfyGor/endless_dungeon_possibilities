using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
   
}

enum RoomPathType
{
    HALL,
    T_SHAPE,
    DEAD_END,
    CROSSROAD
}

enum RoomContent
{
    EMPTY,
    ENEMY,
    TREASURE,
    SPIKE
}