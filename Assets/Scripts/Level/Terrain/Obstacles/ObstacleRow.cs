using UnityEngine;

/// <summary>
/// Manages the number of obstacles in a row
/// </summary>
public class ObstacleRow : MonoBehaviour
{
    [SerializeField]
    [Range(0, 2)]
    int minimumObstacles;
    [SerializeField]
    [Range(1, 2)]
    int maximumObstacles;

    /// <summary>
    /// 
    /// </summary>
    public int NumberOfObstacles { get; private set; }
    /// <summary>
    /// Indicates if this row has obstacles
    /// </summary>
    public bool HasObstacles { get; set; }

    private void Awake()
    {
        minimumObstacles = Mathf.Clamp(minimumObstacles, 0, maximumObstacles); // Makes sure minimum doesn't exceed maximum
        NumberOfObstacles = Random.Range(minimumObstacles, maximumObstacles + 1);
    }
}
