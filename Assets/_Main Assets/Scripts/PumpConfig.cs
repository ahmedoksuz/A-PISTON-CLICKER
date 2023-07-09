using UnityEngine;

[CreateAssetMenu]
public class PumpConfig : ScriptableObject
{
    [SerializeField] private float pumpSize;
    [SerializeField] [MinMaxRange(0, 1)] private float pumpDistance;
    [SerializeField] private float pumpSpeed;
    [SerializeField] private float idlePumpFrequency;
    [SerializeField] private AnimationCurve pumpCurve;

    public float PumpSize => pumpSize;

    public AnimationCurve PumpCurve => pumpCurve;

    public float PumpSpeed => pumpSpeed;

    public float IdlePumpFrequency => idlePumpFrequency;

    public float PumpDistance => pumpDistance;
}