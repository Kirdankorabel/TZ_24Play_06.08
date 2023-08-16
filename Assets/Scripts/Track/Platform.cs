using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Platform : MonoBehaviour
{
    public Transform rootTransform;
    private Rigidbody _rigidbody;
    private PositionYCalculator _positionCalculator;

    private float currentPositionY = 0;
    private bool _enabled = false;

    public event UnityAction<Platform> OnObstacleCollision;

    public float SetCurrentPositionY
    {
        set
        {
            if (value >= currentPositionY)
            {
                _positionCalculator.positionY = value;
            }
            else
            {
                _positionCalculator.StartCalculating(value);
            }
            currentPositionY = value;
        }
    }

    private void Awake()
    {
        _positionCalculator = GetComponent<PositionYCalculator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
        TowerBuilder.OnLosed += () =>
        {
            _rigidbody.useGravity = true;
            _rigidbody.velocity = Vector3.zero;
            _enabled = true;
        };
    }

    private void FixedUpdate()
    {
        if(rootTransform != null && !_enabled) 
            transform.position = new Vector3(rootTransform.position.x, _positionCalculator.positionY, rootTransform.position.z);
    }

    public void Reset()
    {
        _enabled = false;
        _rigidbody.useGravity = false;
        OnObstacleCollision = null;
        currentPositionY = 0;
        _positionCalculator.positionY = 0;
        _positionCalculator.targetPositionY = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "obstacle")
        {
            OnObstacleCollision?.Invoke(this);
            Track.ParentedPlatform(this.gameObject);
            OnObstacleCollision = null;
            _enabled = true;
            StartCoroutine(FallCorutine());
        }
    }

    private IEnumerator FallCorutine()
    {
        yield return new WaitForSeconds(0.1f);
        _rigidbody.useGravity = true;
    }
}
