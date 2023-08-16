using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] private TowerMover _mover;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody[] _rigidbodies;

    void Start()
    {
        DisablePhysics(true);
        TowerBuilder.OnLosed += () =>
        {
            DisablePhysics(false);
            _animator.SetBool("isRunning", false);
            _animator.Play("Idle", -1, 0f);
        };
        UIController.OnRestart += () => DisablePhysics(true);
        _mover.OnFalled += (value) =>
            _animator.SetBool("isFalled", value);
        _mover.OnStarted += () => _animator.SetBool("isRunning", true);
    }

    private void DisablePhysics(bool enable)
    {
        foreach (var rigidbody in _rigidbodies)
        {
            rigidbody.isKinematic = enable;
            rigidbody.velocity = Vector3.zero;
        }
        _animator.enabled = enable;
    }
}
