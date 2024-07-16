using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>Dice</c> describes a dice with specified side.
/// </summary>
public class Dice : MonoBehaviour
{
    private enum Side { One, Two, Three, Four, Five, Six }

    private static readonly Dictionary<Side, Quaternion> SideToRotation = new() {
        {Side.One, Quaternion.Euler(0, 0, 90) },
        {Side.Two, Quaternion.Euler(180, 0, 0) },
        {Side.Three, Quaternion.Euler(-90, 0, 0) },
        {Side.Four, Quaternion.Euler(90, 0, 0) },
        {Side.Five, Quaternion.Euler(0, 0, 0) },
        {Side.Six, Quaternion.Euler(0, 0, -90) },
    };

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform pivot;
    [SerializeField] private MeshRenderer meshRenverer;

    [SerializeField] private Side side;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    /// <summary>
    /// This method sets a dice visible or not.
    /// </summary>
    public void SetVisible(bool visible) => meshRenverer.enabled = visible;

    /// <summary>
    /// This method checks if dice stopped moving.
    /// </summary>
    public bool IsSleeping() => rb.IsSleeping();

    /// <summary>
    /// This method drops dice with preset.
    /// </summary>
    public void Drop(DropPreset dropParameters)
    {
        transform.SetPositionAndRotation(startPosition, startRotation);
        rb.velocity = dropParameters.Velocity;
        rb.angularVelocity = dropParameters.AngularVelocity;
    }

    /// <summary>
    /// This method turns over dice on specified side.
    /// </summary>
    public void TurnOver()
    {
        var currentSide = GetCurrentSide();
        var oldRotation = SideToRotation[currentSide];
        var newRotation = SideToRotation[side];
        pivot.localRotation = Quaternion.Inverse(oldRotation) * newRotation;
    }

    private Side GetCurrentSide()
    {
        if (Vector3.Dot(transform.forward, Vector3.up) > 0.9f) return Side.Three;
        else if (Vector3.Dot(-transform.forward, Vector3.up) > 0.9f) return Side.Four;
        else if (Vector3.Dot(transform.right, Vector3.up) > 0.9f) return Side.One;
        else if (Vector3.Dot(-transform.right, Vector3.up) > 0.9f) return Side.Six;
        else if (Vector3.Dot(transform.up, Vector3.up) > 0.9f) return Side.Five;
        else return Side.Two;
    }
}
