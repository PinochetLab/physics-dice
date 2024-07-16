using UnityEngine;

public class PathPoint
{
    public float DeltaTime { get; }
    public Vector3 Position { get; }
    public Quaternion Rotation { get; }

    public PathPoint(float deltaTime, Vector3 position, Quaternion rotation)
    {
        DeltaTime = deltaTime;
        Position = position;
        Rotation = rotation;
    }
}
