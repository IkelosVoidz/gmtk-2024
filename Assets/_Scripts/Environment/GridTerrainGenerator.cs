using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GridTerrainGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private float _cellScale = 1;
    [SerializeField] private int _maxX = 50;
    [SerializeField] private int _maxY = 10;

    private void Start()
    {
        var points = GenerateRandomPoints();

        var pos = (Vector2)transform.position;
        foreach (var point in points)
        {
            var cell = Instantiate(_cellPrefab, transform);
            cell.transform.localScale = Vector3.one * _cellScale;
            cell.transform.position = pos + new Vector2(point.x, point.y) * _cellScale;
        }
    }

    private List<Vector2> GenerateRandomPoints()
    {
        var random = new Random();
        var points = new List<Vector2>();
        var currentY = 0;

        points.Add(new Vector2(0, currentY));

        
        
        for (var currentX = 1; currentX <= _maxX; currentX++)
        {
            var remainingDistanceToZero = _maxX - currentX;
            int nextY;

            if (Mathf.Abs(currentY) < remainingDistanceToZero && Mathf.Abs(currentY) < _maxY)
            {
                var move = random.Next(-1, 2);
                nextY = currentY + move;
            }
            else
            {
                nextY = currentY + (currentY > 0 ? -1 : 1);
            }

            points.Add(new Vector2(currentX, nextY));
            currentY = nextY;
        }

        return points;
    }
}
