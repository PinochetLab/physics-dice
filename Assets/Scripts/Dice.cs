using System.Collections;
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

    private Vector3 oldPosition;
    private Quaternion oldRotation;

    private bool pathIsRecorded;
    private int currentIndex;
    private float currentTime;

    private readonly List<PathPoint> path = new ();

    private void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    /// <summary>
    /// This method starts to record physics path of dice.
    /// </summary>
    public void StartRecordPath()
    {
        path.Clear();
        pathIsRecorded = true;
    }

    /// <summary>
    /// This method stops record path and plays it.
    /// </summary>
    public void StopRecordPathAndPlay()
    {
        pathIsRecorded = false;
        rb.isKinematic = true;
        currentIndex = 0;
        currentTime = 0;
        oldPosition = startPosition;
        oldRotation = startRotation;
    }

    private void Update()
    {
        if (pathIsRecorded)
        {
            path.Add(new PathPoint(Time.deltaTime, transform.position, transform.rotation));
        } else
        {
            currentTime += Time.deltaTime;
            while (currentIndex <= path.Count - 1 && currentTime > path[currentIndex].DeltaTime)
            {
                currentTime -= path[currentIndex].DeltaTime;
                oldPosition = path[currentIndex].Position;
                oldRotation = path[currentIndex].Rotation;
                currentIndex++;
            }
            if (currentIndex >= path.Count) return;
            var t = currentTime / path[currentIndex].DeltaTime;
            var position = Vector3.Lerp(oldPosition, path[currentIndex].Position, t);
            var rotation = Quaternion.Lerp(oldRotation, path[currentIndex].Rotation, t);
            transform.SetPositionAndRotation(position, rotation);
        }
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
        StopAllCoroutines();
        rb.isKinematic = false;
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
        else if (Vector3.Dot(-transform.up, Vector3.up) > 0.9f) return Side.Two;
        else
        {
            Debug.Log("STOP!");
            return Side.One;
        }
    }
}
