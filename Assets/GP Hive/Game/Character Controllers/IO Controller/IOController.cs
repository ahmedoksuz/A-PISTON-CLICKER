using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class IOController : MonoBehaviour
{
    [SerializeField] private IOControllerConfig config;

    private Joystick joystick;
    private Rigidbody rigidbody;

    [SerializeField] private bool normalizeJoystickInput;
    [SerializeField] private bool useAnimator;

    [ShowIf("useAnimator")] [SerializeField]
    private RuntimeAnimatorController animatorController;

    [ShowIf("useAnimator")] [SerializeField]
    private Avatar avatar;

    [SerializeField] private bool useNavmeshForBoundaries;

    private NavMeshAgent navMeshAgent;

    [ShowIf("useNavmeshForBoundaries")] [SerializeField]
    private float navMeshBoundaryCheckDistance = .1f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();

        if (!useAnimator) return;
        var _animator = gameObject.AddComponent<Animator>();
        _animator.runtimeAnimatorController = animatorController;
        _animator.avatar = avatar;

        var _animatorController = gameObject.AddComponent<IOAnimatorController>();
        if (normalizeJoystickInput)
            _animatorController.SetNormalizedInput();
    }

    private void Start()
    {
        joystick = Joystick.Instance;
    }

    private void FixedUpdate()
    {
        Rotation();
        Movement();
    }

    private void Movement()
    {
        var _input = normalizeJoystickInput ? joystick.Direction.normalized.magnitude : joystick.Direction.magnitude;

        if (_input <= config.MovementThreshold) return;

        var _movementVector = transform.position + config.ForwardSpeed * _input * Time.deltaTime * transform.forward;

        if (useNavmeshForBoundaries)
        {
            if (NavMesh.SamplePosition(_movementVector, out var _hit, navMeshBoundaryCheckDistance, NavMesh.AllAreas))
                rigidbody.MovePosition(_hit.position);
        }
        else
            rigidbody.MovePosition(_movementVector);
    }

    private void Rotation()
    {
        var _input = joystick.Direction;
        if (_input.magnitude <= config.RotationThreshold) return;

        var _dir = Quaternion.Euler(0, Mathf.Atan2(_input.x, _input.y) * 180 / Mathf.PI, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, _dir, Time.deltaTime * config.RotateSpeed);
    }
}