using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IOAnimatorController : MonoBehaviour
{
    private Joystick joystick;
    private Animator animator;
    private bool normalizeJoystickInput;

    private static readonly int InputHash = Animator.StringToHash("input");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        joystick = Joystick.Instance;
    }

    private void Update()
    {
        MovementAnimation();
    }

    private void MovementAnimation()
    {
        var _input = normalizeJoystickInput ? joystick.Direction.normalized.magnitude : joystick.Direction.magnitude;
        animator.SetFloat(InputHash, _input);
    }

    public void SetNormalizedInput()
    {
        normalizeJoystickInput = true;
    }
}