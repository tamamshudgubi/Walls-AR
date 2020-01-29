using UnityEngine;
using System;

public class DestroyByTouch : MonoBehaviour
{
    private int _hitsToDestroy = 10;
    private int _currentHits;

    RaycastHit hit;

    public event Action<Vector3, GameObject, Color> WallDestroyed;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            Fire(hit, (Vector3)touch.position);

            WallStats wall = hit.collider.GetComponent<WallStats>();

            if (wall != null)
            {
                _currentHits++;
                if (_currentHits >= _hitsToDestroy)
                {
                    Destroy(hit.collider.gameObject);
                    WallDestroyed?.Invoke(hit.collider.transform.position, hit.collider.gameObject, hit.collider.GetComponent<Renderer>().material.color);
                }
            }
        }
    }

    private bool Fire(RaycastHit hit, Vector3 position)
    {
        if (Physics.Raycast(position, Vector3.forward, out hit, Mathf.Infinity))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
