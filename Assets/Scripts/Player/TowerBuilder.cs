using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TowerBuilder : MonoBehaviour
{
    [SerializeField] private float _offset;
    [SerializeField] private TowerMover _mover;
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerTriggerController triggerController;

    private Platform _lastPlatform;
    private List<Platform> _platforms = new List<Platform>();
    private List<Vector3> _platformPositions = new List<Vector3>();
    private bool isInCollision;
    private bool _lose;

    public static event Action OnLosed;
    public UnityEvent<int> OnPlatformAdded;
    public event Action OnObstacleCollision;
    public event Action<Vector3> OnTowerUpdate;

    public int PlatformCount => _platforms.Count;
    public float Height => (_platforms.Count) * (1 + _offset);

    private void Start()
    {
        UIController.OnRestart += () => Reset();
        triggerController.OnObstacleTriggered += () => Lose();
    }

    private void Reset()
    {
        foreach(var platform in _platforms)
        {
            platform.OnObstacleCollision -= (platform) => RemovePlatform(platform);
            platform.rootTransform = null;
        }
        _platforms.Clear();
        _platformPositions.Clear();
        transform.position = Vector3.up;
        _mover.SetCurrentPositionY = 1;
        _lose = false;
    }

    public void AddPlatform(Platform platform)
    {
        if (_platforms.Contains(platform) || _lose)
            return;

        _platforms.Add(platform);
        var position = Vector3.up * (1 + _offset) * (_platforms.Count);

        platform.transform.parent = transform;
        platform.rootTransform = _mover.transform;
        platform.SetCurrentPositionY = position.y;
        platform.transform.localPosition = -position;
        platform.OnObstacleCollision += (platform) => RemovePlatform(platform);

        _lastPlatform = platform;
        _platformPositions.Add(position);
        _mover.SetCurrentPositionY = Height + 1;
        OnTowerUpdate?.Invoke(_platformPositions[_platformPositions.Count - 1]);
        OnPlatformAdded?.Invoke(_platforms.Count);
    }


    public void RemovePlatform(Platform platform, bool reset = false)
    {
        if (platform == _lastPlatform && ! _lose)
        {
            Lose();
            return;
        }

        platform.OnObstacleCollision -= (platform) => RemovePlatform(platform);
        platform.rootTransform = null;

        if (!reset)
        {
            _platformPositions.Clear();
            _platforms.Remove(platform);
            for (var i = 0; i < _platforms.Count; i++)
                _platformPositions.Add(Vector3.up * (1 + _offset) * (i));
            if (!isInCollision)
                StartCoroutine(RemovePlatformCorutine(0.1f));
        }
    }

    private void Lose()
    {
        if (_lose)
            return;
        Handheld.Vibrate();
        foreach (var platform in _platforms)
            Track.ParentedPlatform(platform.gameObject);
        _lose = true;
        OnLosed?.Invoke();
    }

    private void FallPlatforms()
    {
        if(_lose) return;
        for (var i = 0; i < _platforms.Count; i++)
            _platforms[i].SetCurrentPositionY = _platformPositions[i].y + 1;
        _mover.SetCurrentPositionY = Height + 1;
        if( _platformPositions.Count > 0)
            OnTowerUpdate?.Invoke(Vector3.up * Height + _platformPositions[ _platformPositions.Count - 1]);
        Handheld.Vibrate();
    }

    public IEnumerator RemovePlatformCorutine(float time)
    {
        isInCollision = true;
        yield return new WaitForSeconds(time); 

        OnObstacleCollision?.Invoke();
        isInCollision = false;
        FallPlatforms();
    }
}
