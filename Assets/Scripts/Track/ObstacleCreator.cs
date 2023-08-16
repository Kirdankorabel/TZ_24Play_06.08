using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObstacleCreator : Singleton<ObstacleCreator>
{
    [SerializeField] private GameObject _obstaclePrefab;
    [SerializeField] private int _defaultCapacity;
    [SerializeField] private int _maxSize;
    [SerializeField] private Walls _walls;

    private ObjectPool<GameObject> _pool;
    private List<GameObject> _obstacles = new List<GameObject>();

    private const int _wallCount = 1;
    private int _elenemtSize;
    private int _platformCount;
    private float _offset;
    private int _lastWall;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            InstantiateObstacle,
            OnGet,
            OnReleas,
            OnDestroyElement,
            false,
            _defaultCapacity,
            _maxSize);

        _elenemtSize = Settings.instance.TrackSettings.elenemtElementSize;
        _platformCount = Settings.instance.TrackSettings.platformCount;
        _offset = (float)_elenemtSize / (float)(_wallCount + _platformCount);

    }

    public IEnumerable<GameObject> CreateWall(Transform trackElementTransform)
    {
        GameObject obstacle;
        _obstacles.Clear();

        for (var i = 0; i < _wallCount; i++)
        {
            var wallId = UnityEngine.Random.Range(0, _walls.GetWalls.Count);
            if( _lastWall == wallId)
                wallId = UnityEngine.Random.Range(0, _walls.GetWalls.Count);
            _lastWall = wallId;
            var wall = _walls.GetWalls[wallId];
            var trackElementPosition = trackElementTransform.position - Vector3.right * _elenemtSize / 2;
            var wallPosition = trackElementPosition + new Vector3(_offset * (i + 1) * (_wallCount + _platformCount - 1) / _wallCount, 1f, 0);
            for (var j = 0; j < wall.obstaclePositions.Count; j++)
            {
                obstacle = _pool.Get();
                obstacle.transform.parent = trackElementTransform;
                obstacle.transform.position = wallPosition + wall.obstaclePositions[j];
                obstacle.gameObject.name = $"{i} + {j}";
                _obstacles.Add(obstacle);
            }
        }

        return _obstacles;
    }

    public void ClearObstacles(IEnumerable<GameObject> obstacles)
    {
        foreach (var obstacle in obstacles)
            _pool.Release(obstacle);
    }

    private void OnApplicationQuit()
    {
        _pool.Dispose();
    }

    #region pool methods
    private GameObject InstantiateObstacle()
    {
        var obstacle = Instantiate(_obstaclePrefab);
        return obstacle;
    }

    private void OnGet(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }
    private void OnReleas(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
    private void OnDestroyElement(GameObject gameObject) { }
    #endregion
}