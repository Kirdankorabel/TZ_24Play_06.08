using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlatformCreator : Singleton<PlatformCreator>
{
    [SerializeField] private GameObject _platformPrefab;
    [SerializeField] private int _defaultPoolCapacity;
    [SerializeField] private int _maxPoolSize;
    private ObjectPool<GameObject> _pool;
    private int _platformCount;
    private int _elenemtSize;
    private int _min, _max;
    private float _offset;

    void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            InstantiateObstacle,
            OnGet,
            OnReleas,
            OnDestroyElement,
            false,
            _defaultPoolCapacity,
            _maxPoolSize);

        _platformCount = Settings.Instance.TrackSettings.platformCount;
        _elenemtSize = Settings.Instance.TrackSettings.elenemtElementSize;
        _min = Settings.Instance.TrackSettings.min;
        _max = Settings.Instance.TrackSettings.max;

        _offset = (float)_elenemtSize / (float)(_platformCount + 1);
    }

    public IEnumerable<GameObject> CreatePlatforms(Transform trackElementTransform)
    {
        GameObject platform;
        List<GameObject> platforms = new List<GameObject>();
        for (var i = 0; i < _platformCount; i++)
        {
            platform = _pool.Get();
            platform.transform.parent = trackElementTransform;
            platform.transform.position = trackElementTransform.position + new Vector3(_offset * i - _elenemtSize / 2f + 1f, 1, Random.Range(_min, _max + 1));
            platforms.Add(platform);
        }
        return platforms;
    }

    private void OnApplicationQuit()
    {
        _pool.Dispose();
    }

    public void ClearPlatforms(IEnumerable<GameObject> platforms)
    {
        foreach (var platform in platforms)
            _pool.Release(platform);
    }

    #region pool methods
    private GameObject InstantiateObstacle()
    {
        var obstacle = Instantiate(_platformPrefab);
        return obstacle;
    }

    private void OnGet(GameObject gameObject)
    {
        gameObject.GetComponent<Platform>().Reset();
        gameObject.SetActive(true);
    }

    private void OnReleas(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    private void OnDestroyElement(GameObject gameObject) { }
    #endregion
}
