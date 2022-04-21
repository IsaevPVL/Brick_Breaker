using UnityEngine;

[CreateAssetMenu(fileName = "Brick Palette", menuName = "Net Breaker/Brick Palette")]
public class BrickPalette : ScriptableObject
{
    [SerializeField] GameObject[] bricks;

    public GameObject GetBrickByIndex(int index){
        return bricks[index];
    }
}
