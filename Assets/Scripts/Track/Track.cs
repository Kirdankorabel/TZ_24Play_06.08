using System;
using UnityEngine;

public class Track : MonoBehaviour
{
    [SerializeField] private TrackElement _trackElementPrefab;
    [SerializeField] private int _trackSize;

    private static float _floatingSize;
    private static TrackElement[] _track;

    private Vector3 _elementScale;

    public static event Action<Vector3> OnTrackUpdated;
    public static event Action OnTrackReseted;

    private void Awake()
    {
        _floatingSize = Settings.Instance.TrackSettings.elenemtElementSize;
        transform.position = - Vector3.down;
        _elementScale = new Vector3(Settings.Instance.TrackSettings.elenemtElementSize, 1, 5);
        OnTrackUpdated += (distance) => Move(distance);

        InstantiateTrack();
    }

    private void Start()
    {
        UIController.OnRestart += () => Reset();
        for (var i = 0; i < _trackSize; i++)
            _track[i].Initialize();
    }

    private void Reset()
    {
        for (var i = 0; i < _trackSize; i++)
            _track[i].Initialize(true);
        OnTrackReseted?.Invoke();
    }

    public static void Parented(Transform child)
    {
        int position = (int)(child.position.x / _floatingSize);
        if (position < _track.Length)
            child.parent = _track[position].transform;
    }
    public static void ParentedPlatform(GameObject platform)
    {
        int position = (int)(platform.transform.position.x / _floatingSize);
        if (position < _track.Length)
            _track[position].AddPlatform(platform);
    }

    private void InstantiateTrack()
    {
        if (_track == null)
            _track = new TrackElement[_trackSize];
        for (var i = 0; i < _trackSize; i++)
        {
            TrackElement trackElement;
            if (_track[i] == null)
                trackElement = Instantiate(_trackElementPrefab, new Vector3(i * _floatingSize, 0, 0), Quaternion.identity, this.transform);
            else
                trackElement = _track[i];
            trackElement.transform.localScale = _elementScale;
            _track[i] = trackElement;

            trackElement.name = $"trackElement_ + {i}";
            trackElement.OnPlayerEnter += () => OnTrackUpdated.Invoke(new Vector3((_trackSize - 1) * _floatingSize, 0, 0));
        }
    }
    private void Move(Vector3 distance)
    {
        var firstElement = _track[0];
        for (var i = 1; i < _trackSize; i++)
        {
            _track[i].gameObject.transform.position = new Vector3((i - 1) * _floatingSize, 0, 0);
            _track[i - 1] = _track[i];
        }
        firstElement.transform.position = distance;
        _track[_trackSize - 1] = firstElement;
        _track[_trackSize - 1].Initialize();
    }
}
