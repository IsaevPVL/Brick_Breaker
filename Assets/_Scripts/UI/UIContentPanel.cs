using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class UIContentPanel: MonoBehaviour
{
    [SerializeField] GameObject background;
    [SerializeField] GameObject loadingWheel;
    [SerializeField] RectTransform canvas;
    [Header("UI Panels")]
    [SerializeField] GameObject files;
    [SerializeField] GameObject terminal;
    [SerializeField] GameObject network;
    [SerializeField] GameObject menu;
    
    [Space, Range(0, 1), SerializeField] float openTime = 0.3f;
    [Range(0, 1), SerializeField] float closeTime = 0.3f;
    [SerializeField] float loadingTime = 0.3f;
    //[Range(0, 1)] public float topPadding;

    float defaultYPosition;
    float defaultYScale;
    float fieldYTop;
    float inventoryYTop;
    float bricksStart;
    GameObject currentPanel;
    Sequence sequence;
    Dictionary<UIState, GameObject> panels;


    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(.1f);   //Waiting for InventoryGrid
        CalculateDefaultBoundaries();
        //sequence = DOTween.Sequence();

        panels = new Dictionary<UIState, GameObject>() {
            {UIState.Gameplay, null},
            {UIState.Files, files},
            {UIState.Terminal, terminal},
            {UIState.Network, network},
            {UIState.Menu, menu},
        };

        canvas.gameObject.SetActive(false);
        foreach (var item in panels)
        {
            if(panels.TryGetValue(item.Key, out GameObject obj))
                obj?.SetActive(false);
        }
    }

    public void SetState(UIState upcomingState, UIState currentState)
    {

        switch (upcomingState)
        {
            case UIState.Files:
                SetBackground(inventoryYTop - defaultYPosition, (bricksStart + inventoryYTop), upcomingState);
                break;

            case UIState.Terminal:
                SetBackground(0, inventoryYTop - defaultYPosition, upcomingState);
                break;

            case UIState.Network:
                SetBackground(0, fieldYTop, upcomingState);
                break;

            case UIState.Menu:
                SetBackground(0, fieldYTop / 2, upcomingState);

                break;

            default:
                SetBackground(0, 0, upcomingState, false);
                break;
        }
    }
    //void SetBackground(float bottomUnits, float topUnits, GameObject stateElement = null, bool isOpening = true)
    void SetBackground(float bottomUnits, float topUnits, UIState upcomingState, bool isOpening = true)
    {
        sequence?.Kill(true);
        sequence = DOTween.Sequence();

        Vector2 canvasSize = canvas.sizeDelta;
        canvasSize.y = topUnits;
        

        sequence.OnStart(() =>
        {   
            if (isOpening){
                loadingWheel.SetActive(true);
                canvas.gameObject.SetActive(true);
                currentPanel?.SetActive(false);
            }else{
                if(currentPanel == files)
                   currentPanel.SetActive(false); 
            }
        });

        sequence.OnComplete(() =>
        {
            currentPanel?.SetActive(false);
            currentPanel = panels[upcomingState];

            if (isOpening)
            {
                loadingWheel.SetActive(false);
                currentPanel?.SetActive(true);
            }else{
                canvas.gameObject.SetActive(false);
            }
        });

        float time;
        if(isOpening){
            sequence.AppendInterval(loadingTime);
            time = openTime;
        }else{
            time = closeTime;
        }

        sequence.Prepend(background.transform.DOMoveY(defaultYPosition + bottomUnits, time));
        sequence.Join(background.transform.DOScaleY(defaultYScale + topUnits, time));
        //sequence.Join(loadingWheel.transform.DOMoveY(defaultYPosition + bottomUnits + (defaultYScale + topUnits) / 2, openTime / 2));
        sequence.Join(canvas.DOAnchorPosY((bottomUnits), time));
        sequence.Join(canvas.DOSizeDelta(canvasSize, time));


    }

    void CalculateDefaultBoundaries()
    {
        transform.position = InventoryGrid.active.inventoryGridLayout.CellToWorld(new Vector3Int(0, 2, 0));

        fieldYTop = GameObject.FindObjectOfType<Boundaries>().corners[1].y - transform.position.y;
        bricksStart = GameObject.FindObjectOfType<FieldGrid>().transform.position.y - transform.position.y - 0.5f;
        Vector3 inventoryTopRightCorner = GameObject.FindObjectOfType<InventoryGrid>().inventoryTopRightCorner;
        inventoryTopRightCorner.y += GameObject.FindObjectOfType<InventoryBorders>().lineWidth * 2;
        Vector3 viewportInventory = Camera.main.WorldToViewportPoint(inventoryTopRightCorner);
        viewportInventory.z -= background.transform.position.z;
        inventoryYTop = Camera.main.ViewportToWorldPoint(viewportInventory).y;

        background.transform.localScale = new Vector3(inventoryTopRightCorner.x * 2, 0, 1);
        defaultYScale = background.transform.lossyScale.y;
        defaultYPosition = background.transform.position.y;

        canvas.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryTopRightCorner.x * 2);
        canvas.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

        //loadingWheel.transform.position += new Vector3(inventoryTopRightCorner.x, 0, background.transform.localPosition.z - 0.01f);
        //loadingWheelDefaultY = loadingWheel.transform.position.y;
    }
}