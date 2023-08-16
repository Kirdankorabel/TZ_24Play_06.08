using UnityEngine;

public class TowerBase : MonoBehaviour
{
    [SerializeField] private TowerBuilder _builder;
    private Transform _root;

    private void Awake()
    {
        _root = _builder.transform;
    }

    private void Start()
    {
        _builder.OnTowerUpdate += (postion) => transform.position = postion;
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(_root.position.x, 1, _root.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Platform>())
            _builder.AddPlatform(other.gameObject.GetComponent<Platform>());       
    }
}
