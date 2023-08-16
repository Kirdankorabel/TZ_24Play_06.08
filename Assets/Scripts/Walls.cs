using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Walls", menuName = "Walls", order = 1)]
public class Walls : ScriptableObject
{
    [SerializeField] private List<Wall> _walls;
    public List<Wall> GetWalls => _walls;
}

[Serializable] public struct Wall
{
    public List<Vector3> obstaclePositions;
}
