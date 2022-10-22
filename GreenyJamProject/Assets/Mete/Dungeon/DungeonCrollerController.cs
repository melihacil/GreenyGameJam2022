using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    up = 0,
    left = 1,
    down = 2,
    right = 3,
    
};

public class DungeonCrollerController : MonoBehaviour
{
    public static List<Vector2Int> positionVisited = new List<Vector2Int>();

    private static readonly Dictionary<Direction, Vector2Int> directionMovementMap = new Dictionary<Direction, Vector2Int>
    {
        { Direction.up, Vector2Int.up },
        { Direction.left, Vector2Int.left },
        { Direction.down, Vector2Int.down },
        { Direction.right, Vector2Int.right }
   };

    public static List<Vector2Int> GenerateDungeon(DungeonGenerationData dungeonData)
    {
        List<DungeonCroller> dungeonCrollers = new List<DungeonCroller>();

        for(int i = 0; i < dungeonData.numberOfCrawlers; i++)
        {
            dungeonCrollers.Add(new DungeonCroller(Vector2Int.zero));
        }

        int iteration = Random.Range(dungeonData.iterationMin, dungeonData.iterationMax);

        for(int i = 0; i < iteration; i++)
        {
            foreach(DungeonCroller dungeonCroller in dungeonCrollers)
            {
                Vector2Int newPos = dungeonCroller.Move(directionMovementMap);
                positionVisited.Add(newPos);
            }
        }

        return positionVisited;

    }

}
