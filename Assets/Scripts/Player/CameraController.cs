using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Camera _camera;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _defaultAngles;

    private float _speed;
    private int _platformsCount;

    private void Awake()
    {
        _speed = Settings.Instance.Speed;
    }

    private void Start()
    {
        UIController.OnRestart += () => SetPlatformsCount(0);
    }

    private void Update()
    {
        if((transform.position- _target.position).sqrMagnitude < 5)
            transform.position = Vector3.MoveTowards(transform.position,  _target.position + _offset, Time.deltaTime);
        else 
            transform.position = _target.position + _offset;
        if(_platformsCount > _camera.gameObject.transform.eulerAngles.x - _defaultAngles.x)
            _camera.gameObject.transform.eulerAngles = Vector3.MoveTowards(_camera.gameObject.transform.eulerAngles, _defaultAngles + new Vector3(_platformsCount, 0, 0), Time.deltaTime);
        else if (_platformsCount < _camera.gameObject.transform.eulerAngles.x - _defaultAngles.x)
            _camera.gameObject.transform.eulerAngles = Vector3.MoveTowards(_camera.gameObject.transform.eulerAngles, _defaultAngles + new Vector3(_platformsCount, 0, 0), Time.deltaTime);
    }

    public void SetPlatformsCount(int platformsCount)
    {
        _platformsCount = platformsCount;
    }
}
