using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackElement : MonoBehaviour
{
    public event Action OnPlayerEnter;

    private List<GameObject> _platforms;
    private List<GameObject> _obstacles;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TowerMover>() && other.gameObject.transform.parent != transform)
        {
            other.gameObject.transform.parent = transform;
            OnPlayerEnter?.Invoke(); 
        }
    }

    public void Initialize(bool reset = false)
    {
        if (_platforms != null && !reset)
            PlatformCreator.Instance.ClearPlatforms(_platforms.Where(platform => platform.transform.parent == transform));
        else if (_platforms != null && reset)
            PlatformCreator.Instance.ClearPlatforms(_platforms);
        _platforms = PlatformCreator.Instance.CreatePlatforms(transform).ToList();
        if (_obstacles != null)
            ObstacleCreator.Instance.ClearObstacles(_obstacles);
        _obstacles = ObstacleCreator.Instance.CreateWall(transform).ToList();
    }

    public void AddPlatform(GameObject platform)
    {
        if(!_platforms.Contains(platform))
            _platforms.Add(platform);
        platform.transform.parent = transform;
    }

    public void Clear()
    {
        if (_platforms != null)
            PlatformCreator.Instance.ClearPlatforms(_platforms);
        if (_obstacles != null)
            ObstacleCreator.Instance.ClearObstacles(_obstacles);
    }
}