using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Net Breaker/Level")]
public class Level : ScriptableObject
{
    public Vector2Int dimensions = new Vector2Int(8,8);

    public Vector3Int[] bricks;

    public void UpdateLayout() {
        bricks = new Vector3Int[dimensions.x * dimensions.y];
    }
}