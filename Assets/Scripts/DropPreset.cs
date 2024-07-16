using UnityEngine;

/// <summary>
/// Struct <c>DropPreset</c> describes initial velocity and initial angular velocity.
/// </summary>
public struct DropPreset
{
    private const float MinAngularSpeed = 100;
    private const float MaxAngularSpeed = 200;
    public Vector3 Velocity { get; }
    public Vector3 AngularVelocity { get; }

    public DropPreset(Vector3 velocity, Vector3 spinVelocity)
    {
        Velocity = velocity;
        AngularVelocity = spinVelocity;
    }

    /// <summary>
    /// This method generates random drop preset.
    /// </summary>
    public static DropPreset GenerateRandom()
    {
        var spinVelocity = Random.insideUnitSphere * Random.Range(MinAngularSpeed, MaxAngularSpeed);
        var velocity = Random.insideUnitCircle;
        return new DropPreset(velocity, spinVelocity);
    }
}
