using UnityEngine;
using System.Collections.Generic;
using System;

public class CylinderCollector : MonoBehaviour
{
    [SerializeField] private DefaultTrackableEventHandler[] _defaultTrackableEventHandler;

    public List<Vector3> CylinderPositions;

    public event Action<List<Vector3>> LastTrackingFounded;
    public event Action CreatingWalls;

    private void OnEnable()
    {
        _defaultTrackableEventHandler = Resources.FindObjectsOfTypeAll<DefaultTrackableEventHandler>();

        foreach (var handler in _defaultTrackableEventHandler)
        {
            handler.TrackingFounded += OnTrackingFounded;
            handler.TrackingLost += OnTrackingLost;
        }
    }

    private void OnDisable()
    {
        foreach (var handler in _defaultTrackableEventHandler)
        {
            handler.TrackingFounded -= OnTrackingFounded;
            handler.TrackingLost += OnTrackingLost;
        }
    }

    private void OnTrackingFounded(Transform position)
    {
        CylinderPositions.Add(position.position);
        LastTrackingFounded?.Invoke(CylinderPositions);
        CreatingWalls?.Invoke();
    }

    private void OnTrackingLost(Transform position)
    {
        if (CylinderPositions.Contains(position.position))
        {
            CylinderPositions.Remove(position.position);
        }
    }
}
