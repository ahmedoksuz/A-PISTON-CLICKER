using UnityEngine;

[CreateAssetMenu(fileName = "IO Config", menuName = "GP Hive Objects/Character Controllers/IO Config")]
public class IOControllerConfig : ScriptableObject
{
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float movementThreshold;
    [SerializeField] private float rotationThreshold;

    public float ForwardSpeed => forwardSpeed;
    public float RotateSpeed => rotateSpeed;

    public float MovementThreshold => movementThreshold;
    public float RotationThreshold => rotationThreshold;
}