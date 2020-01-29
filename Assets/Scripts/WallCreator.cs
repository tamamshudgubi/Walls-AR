using UnityEngine;
using System.Collections.Generic;

public class WallCreator : MonoBehaviour
{
    [SerializeField] private CylinderCollector _cylinderCollector;

    [SerializeField] private DestroyByTouch _destroybyTouch;

    [SerializeField] private GameObject _brickPrefab;

    public List<Vector3> SupportPoles;
    public List<GameObject> WallsOnScene;

    private Vector3 _direction;
    private float _minDistance = 0;

    private void OnEnable()
    {
        _destroybyTouch.WallDestroyed += OnWallDestroyed;
        _cylinderCollector.LastTrackingFounded += OnLastTrackingFounded;
        _cylinderCollector.CreatingWalls += SpawnWall;
    }

    private void OnDisable()
    {
        _destroybyTouch.WallDestroyed -= OnWallDestroyed;
        _cylinderCollector.LastTrackingFounded -= OnLastTrackingFounded;
        _cylinderCollector.CreatingWalls -= SpawnWall;
    }

    private void OnLastTrackingFounded(List<Vector3> positions)
    {
        SupportPoles = positions;
    }

    private void SpawnWall()
    {
        if (SupportPoles.Count >= 2)
        {
            foreach (var pole in SupportPoles)
            {
                for (int i = 0; i < SupportPoles.Count - 1; i++)
                {
                    _direction = pole - SupportPoles[i];
                    _minDistance = Mathf.Sqrt(_direction.x * _direction.y * _direction.z);

                    for (int j = 0; j < _minDistance; j++)
                    {
                        for (int height = 0; height < 5; height++)
                        {
                            GameObject brick = Instantiate(_brickPrefab, new Vector3(pole.x + _direction.x, pole.y + height, pole.z + _direction.z), Quaternion.identity) as GameObject;
                            WallsOnScene.Add(brick);
                            brick.GetComponent<Renderer>().material.color = SetWallColor(_minDistance);
                        }
                    }
                }
            }
        }
    }

    private Color SetWallColor(float minDistance)
    {
        return Color.HSVToRGB(0, minDistance * (minDistance / _brickPrefab.transform.localScale.x), 100);
    }

    private void OnWallDestroyed(Vector3 position, GameObject origin, Color color)
    {
        GameObject wall = Instantiate(origin, position, Quaternion.identity);
        wall.GetComponent<Renderer>().material.color = color;

        WallsOnScene.Add(wall);
    }
}
