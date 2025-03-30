using UnityEngine;

public class ObstacleRow : MonoBehaviour
{
    [SerializeField]
    [Range(0, 2)]
    int minimumObstacles;
    [SerializeField]
    [Range(1, 2)]
    int maximumObstacles;

    public int MinimumObstacles { get { return minimumObstacles; } }
    public int MaximumObstacles { get { return maximumObstacles; } }

    private void Awake() => minimumObstacles = Mathf.Clamp(minimumObstacles, 0, maximumObstacles);
}
