using UnityEngine;

public class Settings : Singleton<Settings>
{
    [SerializeField] private TrackSettings _trackSettings;
    private float _speed = 7;

    public TrackSettings TrackSettings => _trackSettings;
    public float Speed => _speed;
}