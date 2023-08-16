using System;
using UnityEngine;

public class TowerMover : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] int _startState;
    [SerializeField] TowerBuilder _tower;

    private PositionYCalculator positionCalculator;
    private Vector3 _velocity;
    private Vector3 _startPosition;
    private float currentPositionY = 0;
    private int _state;
    private float min, max;
    private bool _isMoving;
    private bool _isAlive;

    public event Action<bool> OnFalled;
    public event Action OnStarted;

    public float SetCurrentPositionY
    {
        set
        {
            if (value >= currentPositionY)
            {
                transform.position = new Vector3(transform.position.x, value, transform.position.z);
                positionCalculator.positionY = value;
            }
            else
            {
                positionCalculator.StartCalculating(value);
            }
            currentPositionY = value;
        }
    }

    private void Awake()
    {        
        _speed = 0;
        _velocity = new Vector3(_speed, 0, 0);
        _startPosition = transform.position;
        positionCalculator = GetComponent<PositionYCalculator>();
        min = Settings.Instance.TrackSettings.min;
        max = Settings.Instance.TrackSettings.max;
    }

    private void Start()
    {
        Track.Parented(transform);
        positionCalculator.OnFalled += (value) => OnFalled?.Invoke(value);
        TowerBuilder.OnLosed += () =>
        {
            _speed = 0;
            _isAlive = false;
        };
        UIController.OnStarted += () =>
        {
            _speed = Settings.Instance.Speed;
            OnStarted?.Invoke();
            _isAlive = true;
        };
        Track.OnTrackReseted += () => Reset();
    }


    private void FixedUpdate()
    {
        if (_isAlive)
        {
            float positionZ = transform.position.z; 
            positionZ = -(Input.mousePosition.x) / Screen.width * 5 + 2.5f;
            if (Input.touchCount > 0)
            {
                positionZ = -(Input.GetTouch(0).position.x) / Screen.width * 5 + 2.5f;
                Debug.LogError(positionZ);
            }
            positionZ = positionZ < min ? min : positionZ;
            positionZ = positionZ > max ? max : positionZ;
            transform.position = new Vector3(transform.position.x, positionCalculator.positionY, transform.position.z) + Vector3.right* _speed *Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, positionZ), _speed * Time.deltaTime);
        }
    }

    private void Reset()
    {
        positionCalculator.positionY = 1;
        positionCalculator.targetPositionY = 1;
        _speed = Settings.Instance.Speed;
        _velocity = new Vector3(_speed, 0, 0);
        transform.position = _startPosition;
        Track.Parented(transform);
        _isMoving = false;
        _isAlive = true;
        OnStarted?.Invoke();
    }
}
