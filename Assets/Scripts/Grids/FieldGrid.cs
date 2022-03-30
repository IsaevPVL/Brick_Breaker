using UnityEngine;

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
        Grid fieldGrid = fieldGridLayout.GetComponent<Grid>();
        Vector3 topRightCorner = boundaries.deeperCorners[1];
        Debug.Log(topRightCorner);
        Vector3 paddlePosition = GameObject.FindGameObjectWithTag("Paddle").transform.position;

        transform.position = new Vector3(-topRightCorner.x, paddlePosition.y + (topRightCorner.y - paddlePosition.y) * freeSpace, paddlePosition.z);

        float fieldWidth = Mathf.Abs(transform.position.x) * 2 - boundaries.lineWidth + fieldGrid.cellGap.x;
        float fieldHeight = Mathf.Abs(topRightCorner.y - transform.position.y + fieldGrid.cellGap.y);

        fieldPadding = new Vector2();
        fieldPadding.x = fieldWidth * horizontalPadding;
        fieldPadding.y = fieldHeight * verticalPadding;
        transform.position = new Vector3(-topRightCorner.x + fieldPadding.x, paddlePosition.y + (topRightCorner.y - paddlePosition.y) * freeSpace - fieldPadding.y, paddlePosition.z);

        float horizontalSector = ((fieldWidth - fieldPadding.x * 2) / fieldDimensions.x);
        float verticalSector = ((fieldHeight - fieldPadding.y * 2) / fieldDimensions.y);
        //SCALING TEST
        //element.localScale = new Vector3(horizontalSector, verticalSector, 0.2f);
        //NewScale?.Invoke(new Vector2(horizontalSector, verticalSector));

        fieldGrid.cellSize = new Vector3(horizontalSector - fieldGrid.cellGap.x, verticalSector - fieldGrid.cellGap.y, 0);

        Vector3 scale = new Vector3(fieldGrid.cellSize.x / 2, fieldGrid.cellSize.y, 1);
        //Vector3 scale = new Vector3(horizontalSector, verticalSector, 1);
        FillGrid(scale);
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
