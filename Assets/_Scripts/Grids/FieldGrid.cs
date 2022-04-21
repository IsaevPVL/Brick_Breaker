using UnityEngine;

//[ExecuteInEditMode]
public class FieldGrid : MonoBehaviour
{
    public static FieldGrid active;

    public GameObject brick;
    public GridLayout fieldGridLayout;
    public Vector2Int fieldDimensions;
    [Range(0, 1)] public float freeSpace;
    [Range(0, 0.2f)] public float horizontalPadding;
    [Range(0, 0.2f)] public float verticalPadding;
    Vector2 fieldPadding;
    Boundaries boundaries;
    Grid fieldGrid;
    float fieldWidth;
    float fieldHeight;
    Vector3 scale;

    [Space] public BrickPalette brickPalette;
    [Space] public Level level;

    void Awake()
    {
        if (active != null && active != this)
        {
            Destroy(this);
        }
        else
        {
            active = this;
        }

        boundaries = GameObject.FindObjectOfType<Boundaries>();
    }

    void Start()
    {
        fieldGrid = fieldGridLayout.GetComponent<Grid>();
        Vector3 topRightCorner = boundaries.deeperCorners[1];
        //Debug.Log(topRightCorner);
        Vector3 paddlePosition = GameObject.FindGameObjectWithTag("Paddle").transform.position;

        transform.position = new Vector3(-topRightCorner.x, paddlePosition.y + (topRightCorner.y - paddlePosition.y) * freeSpace, paddlePosition.z);

        fieldWidth = Mathf.Abs(transform.position.x) * 2 - boundaries.lineWidth + fieldGrid.cellGap.x;
        fieldHeight = Mathf.Abs(topRightCorner.y - transform.position.y + fieldGrid.cellGap.y);

        fieldPadding = new Vector2();
        fieldPadding.x = fieldWidth * horizontalPadding;
        fieldPadding.y = fieldHeight * verticalPadding;
        transform.position = new Vector3(-topRightCorner.x + fieldPadding.x, paddlePosition.y + (topRightCorner.y - paddlePosition.y) * freeSpace - fieldPadding.y, paddlePosition.z);

        //Fill grid with simple blocks
        CalculateScaleWithDimensions(fieldDimensions.x, fieldDimensions.y);
        FillGrid(scale);

    }

    void CalculateScaleWithDimensions(int xDimension, int yDimension){
        float horizontalSector = ((fieldWidth - fieldPadding.x * 2) / xDimension);
        float verticalSector = ((fieldHeight - fieldPadding.y * 2) / yDimension);
        //SCALING TEST
        //element.localScale = new Vector3(horizontalSector, verticalSector, 0.2f);
        //NewScale?.Invoke(new Vector2(horizontalSector, verticalSector));

        fieldGrid.cellSize = new Vector3(horizontalSector - fieldGrid.cellGap.x, verticalSector - fieldGrid.cellGap.y, 0);

        scale = new Vector3(fieldGrid.cellSize.x / 2, fieldGrid.cellSize.y, 1);
        //Vector3 scale = new Vector3(horizontalSector, verticalSector, 1);
    }

    // void RemoveExistingBricks(GameObject obj){
    //     Destroy(obj);
    // }

    public void FillGridFromLevelObject()
    {
        
        CalculateScaleWithDimensions(level.dimensions.x, level.dimensions.y);
        Transform brickHolder = GameObject.FindGameObjectWithTag("Brick Holder").GetComponent<Transform>();

        for (int i = 0; i < level.bricks.Length; i++)
        {
            Vector3 cell = fieldGridLayout.CellToWorld(new Vector3Int(level.bricks[i].x, level.bricks[i].y, 0));

            GameObject currentBrick = GameObject.Instantiate(brickPalette.GetBrickByIndex(level.bricks[i].z), new Vector3(cell.x, cell.y, cell.z + 1f), Quaternion.identity);
            currentBrick.transform.localScale = scale;
            currentBrick.transform.SetParent(brickHolder);

        }
    }

    void FillGrid(Vector3 scale)
    {
        for (int y = 0; y < fieldDimensions.y; y++)
        {
            for (int x = 0; x < fieldDimensions.x; x++)
            {
                Vector3 cell = fieldGridLayout.CellToWorld(new Vector3Int(x, y, 0));

                GameObject currentBrick = GameObject.Instantiate(brick, new Vector3(cell.x, cell.y, cell.z + 1f), Quaternion.identity);
                currentBrick.transform.localScale = scale;
                currentBrick.transform.SetParent(transform);
            }
        }
    }
}
